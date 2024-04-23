using Sirenix.OdinInspector;
using I2.Loc;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{

    public class SkinTMPHandler : SkinHandler<TMPSkinData>
    {
        [SerializeField, Required]
        private TextMeshProUGUI text = default!;

        [CanBeNull]
        public TMPSkinData GetValue(PecanSkinConfigs configs)
        {
            if (string.IsNullOrEmpty(skinKey))
            {
                return null;
            }

            TMPSkinData data;
            return !configs.TMPSkin.TryGet(skinKey, out data) ? null : data;
        }
        
        public override void UpdateSkin(PecanSkinConfigs configs)
        {
            if (string.IsNullOrEmpty(skinKey))
            {
                return;
            }

            TMPSkinData data;
            
            if (!configs.TMPSkin.TryGet(skinKey, out data)) 
                return;
            
            UpdateSkin(data);
        }

        public override void UpdateSkin(TMPSkinData skinData)
        {
            ApplyColor(text, skinData.FontColor);

            if (skinData.FontAsset != null)
            {
                var localize = text.GetComponent<Localize>();
                if (localize != null)
                    localize.SecondaryTerm = $"Font/{skinData.FontAsset.name}";
                
                text.font = skinData.FontAsset;

                if (skinData.PresetMaterial != null)
                {
                    text.fontSharedMaterial = skinData.PresetMaterial;
                }

                text.UpdateFontAsset();
            }
        }

        protected override void LoadSkin(TMPSkinData skinData)
        {
            if (text != null)
            {
                skinData.FontColor = text.color;
                skinData.FontAsset = text.font;

                if (text.fontSharedMaterial == null)
                {
                    skinData.PresetMaterial = text.font.material;
                }
                else
                {
                    skinData.PresetMaterial = text.fontSharedMaterial;
                }
            }
        }
    }
}
