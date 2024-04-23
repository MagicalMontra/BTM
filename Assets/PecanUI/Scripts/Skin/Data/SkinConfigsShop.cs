using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    public partial class SkinConfigs
    {
        [TabGroup("Shop")]
        [Title("Shop Tab", subtitle: "Key: shop_tab", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ShopTabSkinData shopTab;
        public ShopTabSkinData ShopTab => shopTab;

        [TabGroup("Shop")]
        [Title("Item", subtitle: "Key: shop_item", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ShopElementSkinData shopItem;
        public ShopElementSkinData ShopItem => shopItem;

        [TabGroup("Shop")]
        [Title("Item Name Label", subtitle: "Key: shop_item_name_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData shopItemNameLabel;
        public TMPSkinData ShopItemNameLabel => shopItemNameLabel;

        [TabGroup("Shop")]
        [Title("Item Price Label", subtitle: "Key: shop_item_price_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData shopItemPriceLabel;
        public TMPSkinData ShopItemPriceLabel => shopItemPriceLabel;

        [TabGroup("Shop")]
        [Title("Negative Item Price Label", subtitle: "Key: shop_item_price_negative_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData shopItemPriceNegativeLabel;
        public TMPSkinData ShopItemPriceNegativeLabel => shopItemPriceNegativeLabel;

        [TabGroup("Shop")]
        [Title("Item Owned Label", subtitle: "Key: shop_item_owned_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData shopItemOwnedLabel;
        public TMPSkinData ShopItemOwnedLabel => shopItemOwnedLabel;
    }
}