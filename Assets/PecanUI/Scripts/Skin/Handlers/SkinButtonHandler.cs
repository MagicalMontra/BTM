#nullable enable

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinButtonHandler : SkinHandler<ButtonSkinData>
    {
        [SerializeField, Required]
        private Image stroke = default!;

        [SerializeField, Required]
        private Image bottom = default!;

        [SerializeField, Required]
        private Image top = default!;

        [SerializeField, Required]
        private Image highlight = default!;

        [SerializeField, Required]
        private Image navigator = default!;

        [SerializeField]
        private Image? icon = default!;

        public override void UpdateSkin(ButtonSkinData skinData)
        {
            ApplyColor(stroke, skinData.StrokeColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(highlight, skinData.HighlightColor);
            ApplyColor(navigator, skinData.NavigatorColor);

            if (icon != null)
            {
                ApplyColor(icon, skinData.IconColor);
            }
        }

        protected override void LoadSkin(ButtonSkinData skinData)
        {
            skinData.StrokeColor = stroke.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.HighlightColor = highlight.color;
            skinData.NavigatorColor = navigator.color;

            if (icon != null)
            {
                skinData.IconColor = icon.color;
            }
            else
            {
                skinData.IconColor = Color.white;
            }
        }
    }
}
