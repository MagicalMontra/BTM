#nullable enable

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinLoginElementHandler : SkinHandler<LoginElementSkinData>
    {
        [SerializeField, Required]
        private Image stroke = default!;

        [SerializeField, Required]
        private Image bottom = default!;

        [SerializeField, Required]
        private Image top = default!;

        [SerializeField, Required]
        private Image depth = default!;

        [SerializeField, Required]
        private Image navigator = default!;

        [SerializeField]
        private Image? dayBar = default!;

        public override void UpdateSkin(LoginElementSkinData skinData)
        {
            ApplyColor(stroke, skinData.StrokeColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(depth, skinData.DepthColor);
            ApplyColor(navigator, skinData.NavigatorColor);

            if (dayBar != null)
            {
                ApplyColor(dayBar, skinData.DayBarColor);
            }
        }

        protected override void LoadSkin(LoginElementSkinData skinData)
        {
            skinData.StrokeColor = stroke.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.DepthColor = depth.color;
            skinData.NavigatorColor = navigator.color;

            if (dayBar != null)
            {
                skinData.DayBarColor = dayBar.color;
            }
        }
    }
}
