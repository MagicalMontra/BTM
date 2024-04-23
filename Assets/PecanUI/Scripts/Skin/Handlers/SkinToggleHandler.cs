#nullable enable

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinToggleHandler : SkinHandler<ButtonSkinData>
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

        [SerializeField, Required]
        private Image iconOn = default!;

        [SerializeField, Required]
        private Image iconOff = default!;

        public override void UpdateSkin(ButtonSkinData skinData)
        {
            ApplyColor(stroke, skinData.StrokeColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(highlight, skinData.HighlightColor);
            ApplyColor(navigator, skinData.NavigatorColor);

            ApplyColor(iconOn, skinData.IconColor);
            ApplyColor(iconOff, skinData.IconColor);
        }

        protected override void LoadSkin(ButtonSkinData skinData)
        {
            skinData.StrokeColor = stroke.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.HighlightColor = highlight.color;
            skinData.NavigatorColor = navigator.color;

            skinData.IconColor = iconOn.color;
            skinData.IconColor = iconOff.color;
        }
    }
}
