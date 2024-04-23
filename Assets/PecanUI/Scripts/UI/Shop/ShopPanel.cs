using EnhancedUI.EnhancedScroller;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Shop
{
    public class ShopPanel : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller Scroller => scroller;

        [SerializeField]
        private ItemType category;
        public ItemType Category => category;

        //TODO: replace this with your own item data or inject data during initialization
        [SerializeField]
        private ShopElementData[] items;

        [SerializeField]
        private EnhancedScroller scroller;

        [SerializeField]
        private ShopCellView shopCellView;

        //TODO: update by orientation
        [SerializeField]
        private ItemPerRowData itemPerRow;

        // Key: index
        // Value: shop item data list
        private Dictionary<int, List<ShopElementData>> dataLookup = new Dictionary<int, List<ShopElementData>>();
        private float scrollPosition;

        public void Init()
        {
            var tempItems = PecanServices.Instance.GetShopItems(category);
            if (tempItems != null)
            {
                items = tempItems;
            }

            scroller.Delegate = this;

            int numberOfCells = GetNumberOfCells(null);

            for (int i = 0; i < numberOfCells; i++)
            {
                dataLookup[i] = new List<ShopElementData>();
            }

            int count = 0;
            int index = 0;

            foreach (ShopElementData item in items)
            {
                dataLookup[index].Add(items[count]);
                count++;

                if (count % itemPerRow.GetItemPerRow() == 0)
                {
                    index++;
                }
            }
        }

        public float GetCellViewWidth()
        {
            int itemPerRow = this.itemPerRow.GetItemPerRow();
            float elementWidth = shopCellView.ShopElementPrefab.Rect.rect.width;
            float result = (elementWidth * itemPerRow) + ((itemPerRow - 1) * shopCellView.LayoutGroup.spacing);
            return result;
        }

        internal void StoreScrollPosition()
        {
            scrollPosition = scroller.ScrollPosition;
        }

        internal void RestoreScrollPosition()
        {
            scroller.ScrollPosition = scrollPosition;
        }

        internal void ResetScrollPosition()
        {
            scrollPosition = 0f;
            scroller.ScrollPosition = 0f;
        }

        #region IEnhancedScrollerDelegate
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)items.Length / (float)itemPerRow.GetItemPerRow());
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return shopCellView.GetComponent<RectTransform>().rect.height;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            ShopCellView cellView = scroller.GetCellView(shopCellView) as ShopCellView;
            cellView.name = "Cell " + dataIndex.ToString();

            cellView.Init(dataLookup[dataIndex].ToArray(), this);

            return cellView;
        }
        #endregion
    }
}
