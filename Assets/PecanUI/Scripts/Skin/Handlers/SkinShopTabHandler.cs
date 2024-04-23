#nullable enable

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinShopTabHandler : SkinHandler<ShopTabSkinData>
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
        private Image tabDepth = default!;

        [SerializeField, Required]
        private Image tabBorder = default!;

        [SerializeField, Required]
        private Image tabActiveBorder = default!;

        [SerializeField, Required]
        private Image icon = default!;

        [SerializeField, Required]
        private TextMeshProUGUI text = default!;

        public override void UpdateSkin(ShopTabSkinData skinData)
        {
            ApplyColor(stroke, skinData.StrokeColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(navigator, skinData.NavigatorColor);
            ApplyColor(tabDepth, skinData.DepthColor);
            ApplyColor(tabBorder, skinData.TabBorderColor);
            ApplyColor(tabActiveBorder, skinData.TabActiveBorderColor);
            ApplyColor(icon, skinData.IconColor);

            TMPSkinData tmpSkin = skinData.TabLabel;
            ApplyColor(text, tmpSkin.FontColor);

            if (tmpSkin.FontAsset != null)
            {
                text.font = tmpSkin.FontAsset;

                if (tmpSkin.PresetMaterial != null)
                {
                    text.fontSharedMaterial = tmpSkin.PresetMaterial;
                }

                text.UpdateFontAsset();
            }
        }

        protected override void LoadSkin(ShopTabSkinData skinData)
        {
            skinData.StrokeColor = stroke.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.NavigatorColor = navigator.color;
            skinData.DepthColor = tabDepth.color;
            skinData.TabBorderColor = tabBorder.color;
            skinData.TabActiveBorderColor = tabActiveBorder.color;
            skinData.IconColor = icon.color;

            if (text != null)
            {
                skinData.TabLabel.FontColor = text.color;
                skinData.TabLabel.FontAsset = text.font;

                if (text.fontSharedMaterial == null)
                {
                    skinData.TabLabel.PresetMaterial = text.font.material;
                }
                else
                {
                    skinData.TabLabel.PresetMaterial = text.fontSharedMaterial;
                }
            }
        }
    }
}
