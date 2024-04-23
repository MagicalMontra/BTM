using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.UI.RenderManagement
{
    /// <summary>
    /// render pooled objects vertically
    /// </summary>
    public abstract class VerticalPooledRenderer : BasePooledRenderer
    {
        /// <summary>
        /// Call after finish add all content info
        /// Content won't render without calling this
        /// </summary>
        public override void InitContentSizeAndPosition()
        {
            float currentHeight = 0f;
            currentHeight += offset.Top;
            if (CellInfos.Count > 0)
            {
                CellInfos = CellInfos.OrderBy(o => o.Index).ToList();
                foreach (CellInfo item in CellInfos)
                {
                    item.Position.x = offset.Left + item.Position.x - offset.Right;
                    item.Position.y = (-currentHeight);
                    if (item.Type == RenderType.Static)
                    {
                        Debug.Assert(item.RenderedObject != null, "Static render type object cannot be null");
                        item.RenderedObject.anchorMin = Vector2.up;
                        item.RenderedObject.anchorMax = Vector2.up;
                        item.RenderedObject.anchoredPosition = item.Position;
                    }
                    currentHeight += item.CellSize + spacing;
                }
                //cut off last item space
                currentHeight -= spacing;
            }
            currentHeight += offset.Buttom;
            contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentHeight);
            Refresh();
        }

        /// <summary>
        /// indicate whether the item row should be visible or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool ShouldVisible(CellInfo item)
        {
            float viewPointTopY = contentPanel.anchoredPosition.y;
            float viewPortButtomY = contentPanel.anchoredPosition.y + viewportPoint.rect.height;
            return (viewPointTopY <= -item.Position.y + (item.CellSize + (item.CellSize * additionMultiplier))) && (viewPortButtomY >= -(item.Position.y + (item.CellSize * additionMultiplier)));
        }

        /// <summary>
        /// set item position by put item at the center of the row
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        /// <param name="position"></param>
        protected override void SetPosition(CellInfo info, RectTransform item, Vector2 position)
        {
            float alignedY = position.y - info.CellSize * (1 - alignment.y);
            float alignedX = offset.Left + (contentPanel.rect.width - (offset.Right + offset.Left)) * alignment.x;

            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.anchoredPosition = new Vector2(alignedX, alignedY);
        }

        /// <summary>
        /// Calculate the position of an item when being centered
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Vector2 CalculateCenteredPositionOfItemIndex(int index)
        {
            return CalculateCenteredPositionOfItemIndex(index, contentPanel);
        }

        protected Vector2 CalculateCenteredPositionOfItemIndex(int index, RectTransform content)
        {
            CellInfo cellInfo = CellInfos[index];
            float minY = viewportPoint.rect.height - content.rect.height;
            float maxY = 0;
            float targetY = Mathf.Clamp(cellInfo.Position.y + (viewportPoint.rect.height - cellInfo.CellSize) * 0.5f, minY, maxY);
            Vector2 targetanchoredPosition = new Vector2(cellInfo.Position.x, -targetY);
            return targetanchoredPosition;
        }

        protected CellInfo GetCellInfoByIndex(int index)
        {
            return CellInfos[index];
        }
    }
}
