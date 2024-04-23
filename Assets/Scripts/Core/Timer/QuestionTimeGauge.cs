using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.UI;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class QuestionTimeGauge : MonoBehaviour
    {
        private float gaugeSize = float.NegativeInfinity;

        [SerializeField]
        private Color redZoneColor;
        
        [SerializeField]
        private Color greenZoneColor;
        
        [SerializeField] 
        private Image gaugeImage;
        
        [SerializeField] 
        private Image overlayImage;

        [SerializeField] 
        private RectTransform gauge;
        
        [SerializeField] 
        private RectTransform overlay;

        [SerializeField] 
        private Image itemDropImage;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private MMF_Player particleplayrer;

        [SerializeField] 
        private RectTransform.Axis axis = RectTransform.Axis.Horizontal;

        [Inject]
        private GameModeController gameModeController;
        
        [Inject]
        private ItemDropController itemDropController;

        private bool isInitialized;

        private float currentPercentage;
        
        private Sequence sequence;

        private const float redZoneThreshold = 0.3f;

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInitialized)
                return;
            
            overlayImage.DOFade(0f, 0.1f);
            gaugeSize = axis == RectTransform.Axis.Horizontal ? gauge.rect.width : gauge.rect.height;
            isInitialized = true;
        }
        
        public void Show()
        {
            Initialize();
            itemDropImage.sprite = itemDropController.NextDropIcon;
            itemDropImage.rectTransform.anchoredPosition = new Vector2(
                gaugeSize * gameModeController.CurrentGameMode.Settings.ItemDropCurve.Evaluate(1),
                itemDropImage.rectTransform.anchoredPosition.y);
            
            canvasGroup.alpha = 1f;
            gameObject.SetActive(true);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            Initialize();
            itemDropImage.sprite = itemDropController.NextDropIcon;
            itemDropImage.rectTransform.anchoredPosition = new Vector2(
                gaugeSize * gameModeController.CurrentGameMode.Settings.ItemDropCurve.Evaluate(1),
                itemDropImage.rectTransform.anchoredPosition.y);

            canvasGroup.alpha = 0f;
            gameObject.SetActive(true);
            await canvasGroup.DOFade(1f, 0.4f).SetEase(Ease.OutBack).WithCancellation(cancellationToken);
        }

        public void UpdateDropIcon()
        {
            itemDropImage.sprite = itemDropController.NextDropIcon;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            canvasGroup.alpha = 1f;
            await canvasGroup.DOFade(0f, 0.4f).SetEase(Ease.OutBack).WithCancellation(cancellationToken);
            gameObject.SetActive(false);
        }
        
        public void SetItemDropActive(bool isActive)
        {
            itemDropImage.DOFade(isActive ? 1f : 0f, 0.25f);
            itemDropImage.transform.DOScale(isActive ? Vector3.one : Vector3.zero, 0.4f).SetEase(Ease.OutBack);
        }

        public async UniTask SetItemDropActive(bool isActive, CancellationToken cancellationToken)
        {
            itemDropImage.DOFade(isActive ? 1f : 0f, 0.25f);
            await itemDropImage.transform.DOScale(isActive ? Vector3.one : Vector3.zero, 0.4f).SetEase(Ease.OutBack).WithCancellation(cancellationToken);
        }
        
        public void UpdateGauge(float currentPercentage)
        {
            Initialize();

            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
                return;
            }
            
            this.currentPercentage = currentPercentage;
            gaugeImage.color = this.currentPercentage > redZoneThreshold ? greenZoneColor : redZoneColor;
            gauge.SetSizeWithCurrentAnchors(axis, gaugeSize * this.currentPercentage);
        }

        public async UniTask UpdateGaugeAsync(float currentPercentage, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            Initialize();
            sequence?.Kill();
            sequence = DOTween.Sequence();
            this.currentPercentage = currentPercentage;
            overlay.SetSizeWithCurrentAnchors(axis, gaugeSize * this.currentPercentage);
            sequence.AppendCallback(() =>
            {
                particleplayrer.PlayFeedbacks();
            });
            sequence.Append(overlayImage.DOFade(1f, 0.25f));
            sequence.AppendCallback(() =>
            {
                gaugeImage.color = this.currentPercentage > redZoneThreshold ? greenZoneColor : redZoneColor;
                gauge.SetSizeWithCurrentAnchors(axis, gaugeSize * this.currentPercentage);
            });
            sequence.Append(overlayImage.DOFade(0f, 0.5f));
            sequence.AppendInterval(0.5f);

            await sequence.Play().WithCancellation(cancellationToken);
        }
    }
}