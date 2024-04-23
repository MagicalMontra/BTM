#nullable enable

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinLeaderboardTagHandler : SkinHandler<LeaderboardTagSkinData>
    {
        [SerializeField, Required]
        private Image outline = default!;

        [SerializeField, Required]
        private Image bottom = default!;

        [SerializeField, Required]
        private Image top = default!;

        [SerializeField, Required]
        private Image depth = default!;

        [SerializeField, Required]
        private TextMeshProUGUI text = default!;

        public override void UpdateSkin(LeaderboardTagSkinData skinData)
        {
            ApplyColor(outline, skinData.OutlineColor);
            ApplyColor(bottom, skinData.BottomColor);
            ApplyColor(top, skinData.TopColor);
            ApplyColor(depth, skinData.DepthColor);

            ApplyColor(text, skinData.Label.FontColor);

            if (skinData.Label.FontAsset != null)
            {
                text.font = skinData.Label.FontAsset;

                if (skinData.Label.PresetMaterial != null)
                {
                    text.fontSharedMaterial = skinData.Label.PresetMaterial;
                }

                text.UpdateFontAsset();
            }
        }

        protected override void LoadSkin(LeaderboardTagSkinData skinData)
        {
            skinData.OutlineColor = outline.color;
            skinData.BottomColor = bottom.color;
            skinData.TopColor = top.color;
            skinData.DepthColor = depth.color;

            if (text != null)
            {
                skinData.Label.FontColor = text.color;
                skinData.Label.FontAsset = text.font;

                if (text.fontSharedMaterial == null)
                {
                    skinData.Label.PresetMaterial = text.font.material;
                }
                else
                {
                    skinData.Label.PresetMaterial = text.fontSharedMaterial;
                }
            }
        }
    }
}
