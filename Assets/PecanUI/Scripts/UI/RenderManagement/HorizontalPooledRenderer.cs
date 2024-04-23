using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HotPlay.UI.RenderManagement
{
    /// <summary>
    /// Render pooled objects horizontal
    /// </summary>
    public abstract class HorizontalPooledRenderer : BasePooledRenderer
    {
        public override void InitContentSizeAndPosition()
        {
            float currentWidth = 0f;
            currentWidth += offset.Left;

            if (CellInfos.Count > 0)
            {
                CellInfos = CellInfos.OrderBy(o => o.Index).ToList();

                foreach (CellInfo item in CellInfos)
                {
                    item.Position.x = currentWidth;
                    item.Position.y = offset.Top + item.Position.y - offset.Buttom;
                    if (item.Type == RenderType.Static)
                    {
                        Debug.Assert(item.RenderedObject != null, "Static render type object cannot be null");
                        item.RenderedObject.anchorMin = Vector2.up;
                        item.RenderedObject.anchorMax = Vector2.up;
                        item.RenderedObject.anchoredPosition = item.Position;
                    }
                    currentWidth += item.CellSize + spacing;
                }

                //cut off last item space
                currentWidth -= spacing;
            }

            currentWidth += offset.Right;
            contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
            Refresh();
        }

        protected override void SetPosition(CellInfo info, RectTransform item, Vector2 position)
        {
            float alignedX = position.x + (info.CellSize * alignment.x);
            float alignedY = offset.Top - (contentPanel.rect.height - (offset.Buttom + offset.Top)) * alignment.y;

            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.anchoredPosition = new Vector2(alignedX, alignedY);
        }

        protected override bool ShouldVisible(CellInfo item)
        {
            float leftBound = viewportPoint.anchoredPosition.x;
            float rightBound = viewportPoint.anchoredPosition.x + viewportPoint.rect.width;
            float contentItemPos = contentPanel.anchoredPosition.x + item.Position.x;
            bool isInLeftBound = leftBound <= contentItemPos + (item.CellSize + ((item.CellSize + spacing) * additionMultiplier));
            bool isInRightBound = rightBound >= contentItemPos - ((item.CellSize + spacing) * additionMultiplier);

            return isInRightBound && isInLeftBound;
        }
    }
}
