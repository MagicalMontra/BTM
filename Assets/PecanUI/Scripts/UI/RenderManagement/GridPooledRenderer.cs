using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.UI.RenderManagement
{
    /// <summary>
    /// render pooled object in a grid
    /// </summary>
    public abstract class GridPooledRenderer : VerticalPooledRenderer
    {
        /// <summary>
        /// number of columns in each row
        /// </summary>
        public int Columns => columns;

        /// <summary>
        /// horizontal space between items
        /// </summary>
        public float HorizontalSpace => horizontalSpace;

        [SerializeField]
        private int columns;

        [SerializeField]
        private float horizontalSpace;

        /// <summary>
        /// add cell items information
        /// each call will start new row even if previously added row is not full
        /// </summary>
        /// <param name="height"></param>
        /// <param name="count"></param>
        /// <param name="payload"></param>
        public void AddItems(float height, int count, object payload = null)
        {
            int rows = Mathf.CeilToInt((float)count / columns);
            for (int i = 0; i < rows; i++)
            {
                int remainingItems = count - (i * columns);
                int rowColumns = Mathf.Min(remainingItems, columns);
                GridPooledRowInfo gridRow = new GridPooledRowInfo(CurrentIndex, rowColumns, payload);
                AddCellInfo(height, gridRow);
            }
        }
        
        /// <summary>
        /// create cell item for display
        /// </summary>
        /// <param name="rowInfo"></param>
        /// <param name="columnIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        protected abstract RectTransform CreateGridContent(CellInfo rowInfo, int columnIndex, object payload);

        /// <summary>
        /// remove cell item no longer visible
        /// </summary>
        /// <param name="item"></param>
        protected abstract void RemoveGridContent(RectTransform item);

        /// <summary>
        /// create row content for display
        /// this is normally not neccessary for grid only rendering
        /// but can be overridden for grid and non-grid mixed rendering
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override RectTransform CreateCellContent(CellInfo info)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// remove row content no longer visible
        /// this is normally not neccessary for grid only rendering
        /// but can be overridden for grid and non-grid mixed rendering
        /// </summary>
        /// <param name="info"></param>
        protected override void RemoveCellContent(CellInfo info)
        {
            throw new System.NotImplementedException();
        }

        protected sealed override void AddVisibleCellContent(CellInfo info)
        {
            if (CurrentRendering.Contains(info))
            {
                return;
            }
            
            GridPooledRowInfo gridRow = info.Payload as GridPooledRowInfo;
            if (gridRow == null)
            {
                base.AddVisibleCellContent(info);
            }
            else
            {
                for(int i = 0; i < gridRow.ColumnCount; i++)
                {
                    RectTransform item = CreateGridContent(info, i, gridRow.Payload);
                    SetPosition(info, i, item);
                    gridRow.Items.Add(item);
                }

                info.RenderedObject = gridRow.Items[0];
                CurrentRendering.Add(info);
            }
        }

        protected sealed override void RemoveInvisibleCellContent(CellInfo info)
        {
            if (!CurrentRendering.Contains(info))
            {
                return;
            }

            GridPooledRowInfo gridRow = info.Payload as GridPooledRowInfo;
            if (gridRow == null)
            {
                base.RemoveInvisibleCellContent(info);
            }
            else
            {
                for(int i = 0; i < gridRow.Items.Count; i++)
                {
                    RemoveGridContent(gridRow.Items[i]);
                }
                gridRow.Items.Clear();
                CurrentRendering.Remove(info);
            }
        }

        /// <summary>
        /// set item position by put item at specific alignment in its cell
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        /// <param name="position"></param>
        private void SetPosition(CellInfo info, int columnIndex, RectTransform item)
        {
            float alignedY = info.Position.y - info.CellSize * (1 - alignment.y);
            float cellSize = (contentPanel.rect.width - (offset.Right + offset.Left)) / Columns;
            float spaces = horizontalSpace * columnIndex;
            float alignedX = offset.Left + spaces + (cellSize * columnIndex + alignment.x);

            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.anchoredPosition = new Vector2(alignedX, alignedY);
        }
    }

    /// <summary>
    /// grid row rendering information
    /// </summary>
    public class GridPooledRowInfo
    {
        /// <summary>
        /// custom payload
        /// </summary>
        public readonly object Payload;

        /// <summary>
        /// row index start from 0
        /// </summary>
        public readonly int RowIndex;

        /// <summary>
        /// number of column in this row
        /// </summary>
        public readonly int ColumnCount;

        /// <summary>
        /// cell items rendered on this row
        /// </summary>
        /// <typeparam name="RectTransform"></typeparam>
        /// <returns></returns>
        public readonly List<RectTransform> Items = new List<RectTransform>();

        public GridPooledRowInfo(int rowIndex, int columnCount, object payload)
        {
            RowIndex = rowIndex;
            ColumnCount = columnCount;
            Payload = payload;
        }
    }
}
