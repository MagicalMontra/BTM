using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI;
using HotPlay.Utilities;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{

    public class AnswerComponent : MonoBehaviour, IGameUIComponent
    {
        [SerializeField]
        private UIButton button;
        
        [SerializeField]
        private TextMeshProUGUI text;

        [Inject]
        private AnswerValidator validator;
        
        [SerializeField]
        private MMF_Player[] wrongPlayers;

        [SerializeField]
        private MMF_Player[] correctPlayers;
        
        [Inject]
        private PauseController pauseController;

        private bool isClicked;
        private bool isInitialized;
        private Tween tween;
        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = transform.localScale;
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(Answer);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(Answer);
        }

        public async UniTask Show(string sText)
        {
            button.interactable = true;
            isClicked = false;
            text.SetText(sText);
            
            foreach (var player in wrongPlayers)
                player.StopFeedbacks();
            
            foreach (var player in correctPlayers)
                player.StopFeedbacks();
            
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            tween?.Kill();
            tween = transform.DOScale(originalScale, 0.4f).SetEase(Ease.OutBack);
            await UniTask.Yield();
        }

        public async UniTask Hide()
        {
            button.interactable = false;
            tween?.Kill();
            transform.localScale = originalScale;
            var answer = ParseAnswer();
            if (answer.HasValue)
                await PlayAnswerAnimation(answer.Value);
            
            tween = transform.DOScale(Vector3.zero, 0.4f);
            await tween;
            gameObject.SetActive(false);
            await UniTask.Yield();
        }

        public void Answer()
        {
            if (pauseController.IsPause)
                return;
            
            isClicked = true;
            var answer = ParseAnswer();
            validator.Answer(answer);
        }

        private async UniTask PlayAnswerAnimation(int answer)
        {
            var sequence = DOTween.Sequence();
            
            if (!isClicked)
            {
                sequence.AppendInterval(0.6f);
                await sequence.Play();
                return;
            }

            if (validator.CheckAnswer(answer))
            {
                foreach (var player in correctPlayers)
                    player.PlayFeedbacks();

                sequence.AppendInterval(0.6f);
                await sequence.Play();
                
                return;
            }

            foreach (var player in wrongPlayers)
                player.PlayFeedbacks();

            sequence.AppendInterval(0.6f);
            await sequence.Play();
        }

        private int? ParseAnswer()
        {
            if (Int32.TryParse(text.text, out var value))
                return value;

            return null;
        }
    }
}