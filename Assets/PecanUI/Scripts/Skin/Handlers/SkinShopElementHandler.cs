#nullable enable

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinShopElementHandler : SkinHandler<ShopElementSkinData>
    {
        [SerializeField, Required]
        private Image stroke = default!;

        [SerializeField, Required]
        private Image bottom = default!;

        [SerializeField, Required]
        private Image top = default!;

        [SerializeField, Required]
        private Image navigator = default!;

        [SerializeField, Required]
        private Image depth = default!;

        [SerializeField, Required]
        private Image nameBackground = default!;

        [SerializeField, Required]
        private Image priceBackground = default!;

        public override void UpdateSkin(ShopElementSkinData skinData)
        {
            ApplyColor(stroke, skinData.StrokeColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(navigator, skinData.NavigatorColor);
            ApplyColor(depth, skinData.DepthColor);
            ApplyColor(nameBackground, skinData.NameBackground);
            ApplyColor(priceBackground, skinData.PriceBackground);
        }

        protected override void LoadSkin(ShopElementSkinData skinData)
        {
            skinData.StrokeColor = stroke.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.NavigatorColor = navigator.color;
            skinData.DepthColor = depth.color;
            skinData.NameBackground = nameBackground.color;
            skinData.PriceBackground = priceBackground.color;
        }
    }
}
