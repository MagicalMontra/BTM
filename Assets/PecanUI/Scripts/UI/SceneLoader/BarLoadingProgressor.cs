using HotPlay.PecanUI.Skin;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.SceneLoader
{
    public class BarLoadingProgressor : LoadingProgressor
    {
        [SerializeField]
        private RectTransform fill;

        [SerializeField]
        private TextMeshProUGUI percentageText;

        [SerializeField]
        private BaseSkinHandler[] skinHandlers;

        private float ySize;

        private void Awake()
        {
            ySize = fill.rect.width;

            if (!PecanServices.HasInstance)
                return;
            
            foreach (var skin in skinHandlers)
            {
                skin.UpdateSkin(PecanServices.Instance.SkinConfigs);
            }
        }

        public override void SetProgress(float percentage)
        {
            percentageText.SetText($"{percentage * 100:N0}%");
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ySize * percentage);
        }
    }
}