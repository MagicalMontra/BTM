#nullable enable

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinPanelHandler : SkinHandler<PanelSkinData>
    {
        [SerializeField, Required]
        private Image background = default!;

        [SerializeField]
        private Image outline = default!;

        public override void UpdateSkin(PanelSkinData skinData)
        {
            ApplyColor(background, skinData.BackgroundColor);

            if (outline != null)
            {
                ApplyColor(outline, skinData.OutlineColor);
            }
        }

        protected override void LoadSkin(PanelSkinData skinData)
        {
            skinData.BackgroundColor = background.color;

            if (outline != null)
            {
                skinData.OutlineColor = outline.color;
            }
        }
    }
}
