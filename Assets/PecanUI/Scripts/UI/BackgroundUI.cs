using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI
{
    public class BackgroundUI : MonoBehaviour
    {
        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private AspectRatioFitter ratioFitter;

        private void Awake()
        {
            backgroundImage.preserveAspect = true;
            Sprite latestBackground = PecanServices.Instance.LatestBackgroundUI;

            if (latestBackground != null)
            {
                SetBackground(latestBackground);
            }

            PecanServices.Instance.BackgroundUIChanged += SetBackground;
        }

        private void SetBackground(Sprite sprite)
        {
            backgroundImage.enabled = true;
            backgroundImage.sprite = sprite;
            ratioFitter.aspectRatio = sprite.textureRect.width / sprite.textureRect.height;
        }

        private void OnDestroy()
        {
            if (PecanServices.HasInstance)
            {
                PecanServices.Instance.BackgroundUIChanged -= SetBackground;
            }
        }
    }
}
