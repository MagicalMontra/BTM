using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Shop
{
    public class ShopCellView : EnhancedScrollerCellView
    {
        public ShopElement ShopElementPrefab => itemPrefab;
        public HorizontalLayoutGroup LayoutGroup => layoutGroup;

        [SerializeField]
        private ShopElement itemPrefab;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private HorizontalLayoutGroup layoutGroup;

        [SerializeField]
        private int maxItemCount;

        private ShopElement[] shopItems;

        private void Awake()
        {
            shopItems = new ShopElement[maxItemCount];
            for(int i = 0; i < maxItemCount; i++)
            {
                ShopElement shopItem = Instantiate<ShopElement>(itemPrefab, rectTransform);
                shopItems[i] = shopItem;
                shopItems[i].gameObject.SetActive(false);
            }
        }

        public void Init(ShopElementData[] dataArray, ShopPanel shopPanel)
        {
            Debug.Assert(dataArray.Length <= maxItemCount, "Data exceed the maximum range of container");
            int count = 0;

            RefreshItems();

            foreach (ShopElementData data in dataArray)
            {
                ShopElement shopItem = shopItems[count];

                bool isEquipped = PecanServices.Instance.IsEquipped(data.Id, shopPanel.Category);
                bool isOwned = PecanServices.Instance.IsOwned(data.Id, shopPanel.Category);

                data.SetIsEquipped(isEquipped);
                data.SetIsOwned(isOwned);
                shopItem.Init(data, shopPanel, this);
                shopItem.gameObject.SetActive(true);
                count++;

                if(count > shopItems.Length)
                {
                    break;
                }
            }
        }

        public override void RefreshCellView()
        {
            base.RefreshCellView();
            RefreshItems();
        }

        private void RefreshItems()
        {
            foreach (ShopElement item in shopItems)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
