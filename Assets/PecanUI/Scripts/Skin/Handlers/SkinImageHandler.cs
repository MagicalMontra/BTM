using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public class SkinImageHandler : SkinHandler<ImageSkinData>
    {
        [SerializeField]
        private Image image;

        [CanBeNull]
        public ImageSkinData GetValue(PecanSkinConfigs configs)
        {
            if (string.IsNullOrEmpty(skinKey))
            {
                return null;
            }

            ImageSkinData data;
            return !configs.ImageSkin.TryGet(skinKey, out data) ? null : data;
        }

        public override void UpdateSkin(PecanSkinConfigs configs)
        {
            if (string.IsNullOrEmpty(skinKey))
            {
                return;
            }

            ImageSkinData data;
            if (!configs.ImageSkin.TryGet(skinKey, out data)) 
                return;
            
            UpdateSkin(data);
        }
        
        public override void UpdateSkin(ImageSkinData skinData)
        {
            ApplyColor(image, skinData.Color);
        }

        protected override void LoadSkin(ImageSkinData skinData)
        {
            skinData.Color = image.color;
        }
    }
}