using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.UI.RenderManagement
{
    public abstract class BasePooledRenderer : MonoBehaviour
    {
        /// <summary>
        /// Generate - Item content that generate from pool when visible and return when invisible
        /// Static - Not pool object but include in content. Will not disappear ex.header
        /// </summary>
        public enum RenderType
        {
            Generate = 0,
            Static = 1
        }

        public class CellInfo
        {
            /// <summary>
            /// Index of this item in content list. Use for sorting.
            /// </summary>
            public int Index;

            /// <summary>
            /// Item rect transform size. Use as width if horizontal and height for vertical.
            /// </summary>
            public float CellSize;

            /// <summary>
            /// Item position in viewpoint. Viewpoint start from Top-Left. So position Y will be minus.
            /// </summary>
            public Vector2 Position;

            /// <summary>
            /// Reference of item when rendered
            /// </summary>
            public RectTransform RenderedObject;

            /// <summary>
            /// Type of render
            /// </summary>
            public RenderType Type;

            /// <summary>
            /// Payload of data
            /// </summary>
            public object Payload;
        }

        [System.Serializable]
        public struct RenderOffset
        {
            public float Left;
            public float Right;
            public float Top;
            public float Buttom;
        }

        /// <summary>
        /// Panel use as content parent
        /// </summary>
        public RectTransform ContentPanel => contentPanel;

        /// <summary>
        /// Rect that stores animating cells (render on top of static cells)
        /// </summary>
        public RectTransform AnimatingCellsParentRect => animatingCellsParentRect;

        /// <summary>
        /// Content offset in ContentPanel
        /// </summary>
        public RenderOffset Offset => offset;

        /// <summary>
        /// Cell spacing in ContentPanel
        /// </summary>
        public float Spacing => spacing;

        /// <summary>
        /// Current index of content. Used only in AddContentInfo and AddStaticContent.
        /// </summary>
        protected int CurrentIndex { get; private set; }

        /// <summary>
        /// List of all cell info
        /// </summary>
        protected List<CellInfo> CellInfos = new List<CellInfo>();

        /// <summary>
        /// Item that currently render
        /// </summary>
        protected List<CellInfo> CurrentRendering = new List<CellInfo>();

        [SerializeField]
        protected Vector2 alignment;

        [SerializeField]
        protected RenderOffset offset;

        [SerializeField]
        protected float spacing = 0f;

        [SerializeField]
        protected RectTransform viewportPoint;

        [SerializeField]
        protected RectTransform contentPanel;

        [SerializeField]
        protected RectTransform animatingCellsParentRect;

        [SerializeField]
        protected ScrollRect scroll;

        /// <summary>
        /// Number of cell for render as addition first and last cell.
        /// </summary>
        [SerializeField]
        protected int additionMultiplier;

        /// <summary>
        /// Item that are waiting for rendering (if not rendered)
        /// </summary>
        private List<CellInfo> pendingRendering = new List<CellInfo>();

        /// <summary>
        /// Add multiple content information sharing same payload
        /// </summary>
        /// <param name="cellSize">Use as width if horizontal and height for vertical</param>
        /// <param name="count">Amount of items</param>
        /// <param name="payload"></param>
        public void AddCellInfo(float cellSize, int count, object payload = null)
        {
            for (int i = 0; i < count; i++)
            {
                AddCellInfo(cellSize, payload);
            }
        }

        /// <summary>
        /// add content with its associated payload
        /// </summary>
        /// <param name="size">Use as width if horizontal and height for vertical</param>
        /// <param name="payload"></param>
        public void AddCellInfo(float size, object payload)
        {
            CellInfos.Add(new CellInfo() { Index = CurrentIndex, CellSize = size, Type = RenderType.Generate, Payload = payload });
            CurrentIndex++;
        }

        /// <summary>
        /// Static item not pool object but include in content. Will not disappear ex.header
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="cellSize">Use as width if horizontal and height for vertical</param>
        public void AddStaticCell(RectTransform transform, float cellSize)
        {
            CellInfo item = new CellInfo() { Index = CurrentIndex, CellSize = cellSize, Type = RenderType.Static, RenderedObject = transform };
            CellInfos.Add(item);
            CurrentRendering.Add(item);
            CurrentIndex++;
        }

        /// <summary>
        /// Get size of a cell in the particular index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetCellSize(int index)
        {
            return CellInfos[index].CellSize;
        }

        /// <summary>
        /// Get current visible rect transforms inside content panel
        /// </summary>
        /// <returns></returns>
        public List<RectTransform> GetVisibleCellRects()
        {
            return new List<RectTransform>(contentPanel.GetComponentsInChildren<RectTransform>());
        }

        /// <summary>
        /// Call after finish add all content info
        /// Content won't render without calling this
        /// </summary>
        public abstract void InitContentSizeAndPosition();

        /// <summary>
        /// Refresh current render items
        /// </summary>
        public void Refresh()
        {
            ClearContent();
            UpdateVisibleContent();
        }

        /// <summary>
        /// Clear rendered content and cell information
        /// </summary>
        public virtual void Clear()
        {
            CurrentIndex = 0;
            ClearContent();
            CurrentRendering.Clear();
            CellInfos.Clear();
        }

        /// <summary>
        /// Enable or disable the interactable properties of the scroll component
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public void SetScrollInteractable(bool horizontal, bool vertical)
        {
            scroll.horizontal = horizontal;
            scroll.vertical = vertical;
        }

        public void StopScrollMovement()
        {
            scroll.StopMovement();
        }

        public bool IsVerticalInteractable()
        {
            return scroll.vertical;
        }

        /// <summary>
        /// create cell item for displaying
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected abstract RectTransform CreateCellContent(CellInfo info);

        /// <summary>
        /// remove cell item no longer visible
        /// </summary>
        /// <param name="info"></param>
        protected abstract void RemoveCellContent(CellInfo info);

        /// <summary>
        /// render cell item created in CreateCellContent at cell position
        /// </summary>
        /// <param name="info"></param>
        protected virtual void AddVisibleCellContent(CellInfo info)
        {
            if (CurrentRendering.Contains(info))
            {
                return;
            }

            info.RenderedObject = CreateCellContent(info);
            CurrentRendering.Add(info);
            SetPosition(info, info.RenderedObject, info.Position);
        }

        /// <summary>
        /// remove cell item by calling RemoveCellContent
        /// </summary>
        /// <param name="info"></param>
        protected virtual void RemoveInvisibleCellContent(CellInfo info)
        {
            if (!CurrentRendering.Contains(info))
            {
                return;
            }

            RemoveCellContent(info);
            info.RenderedObject = null;
            CurrentRendering.Remove(info);
        }

        /// <summary>
        /// indicate whether the item cell should be visible or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract bool ShouldVisible(CellInfo item);

        /// <summary>
        /// called whenever a scrolling happen after item cell visibility resolved
        /// </summary>
        protected virtual void OnScrolled() { }

        /// <summary>
        /// Set item position data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        /// <param name="position"></param>
        protected abstract void SetPosition(CellInfo info, RectTransform item, Vector2 position);

        private void Start()
        {
            Debug.Assert(scroll != null, "Scroll Rect for render controller cannot be null.");
            scroll.onValueChanged.AddListener((value) => UpdateVisibleContent());
        }

        private void ClearContent()
        {
            for (int i = CurrentRendering.Count - 1; i >= 0; i--)
            {
                if (CurrentRendering[i].Type == RenderType.Generate)
                {
                    RemoveInvisibleCellContent(CurrentRendering[i]);
                }
            }
        }

        /// <summary>
        /// render visible item cells
        /// and remove item cells no longer visible
        /// called whenever a scrolling happen
        /// </summary>
        protected void UpdateVisibleContent()
        {
            bool visibleRangeStarted = false;

            //create list of rendering items and immediately remove removing items
            //this allow pool object to be return and re-rent without disable the object
            foreach (CellInfo item in CellInfos)
            {
                if (item.Type == RenderType.Static)
                    continue;

                bool visible = ShouldVisible(item);

                if (visible)
                {
                    visibleRangeStarted = true;
                    pendingRendering.Add(item);
                }
                else
                {
                    if (item.RenderedObject != null)
                    {
                        RemoveInvisibleCellContent(item);
                    }
                    else if (visibleRangeStarted) //null RenderedObject after startRange meant there won't be any RenderedObject after this
                    {
                        break;
                    }
                }
            }

            foreach (CellInfo item in pendingRendering)
            {
                AddVisibleCellContent(item);
            }

            pendingRendering.Clear();

            OnScrolled();
        }
    }
}
