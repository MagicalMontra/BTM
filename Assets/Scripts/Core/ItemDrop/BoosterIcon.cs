using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    public class BoosterIcon : MonoBehaviour
    {
        public ItemDropTypeEnum Type => type;

        [SerializeField]
        private ItemDropTypeEnum type;

        [SerializeField]
        private Animator boosterAnimator;

        [SerializeField]
        private TextMeshProUGUI timerText;
        
        private const string boosterEffectAnim = "Activate";
        private const string boosterActivationAnim = "Idle";
        private const string boosterDisappearAnim = "Disappear";

        private CancellationTokenSource disableCancellationToken;

        private void Awake()
        {
            disableCancellationToken = new CancellationTokenSource();
        }

        public void PlayActivateAnimation()
        {
            disableCancellationToken?.Cancel();
            var count = 0f;
            timerText.SetText(string.Empty);

            if (boosterAnimator != null)
            {
                boosterAnimator.Play(boosterActivationAnim, -1, 0f);

                while (count < boosterAnimator.GetCurrentAnimatorStateInfo(0).length)
                {
                    count += Time.deltaTime;
                    
                    if (!gameObject.activeInHierarchy)
                        gameObject.SetActive(true);
                }
            }
        }
        
        public void PlayEffectAnimation()
        {
            if (boosterAnimator != null)
                boosterAnimator.Play(boosterEffectAnim, -1, 0f);
        }

        public void OnTimerTick(ITimer timer)
        {
            timerText.SetText($"{Mathf.RoundToInt(timer.Counter):D2}s");
        }

        public async UniTaskVoid PlayDisappearAnimation()
        {
            if (boosterAnimator != null)
            {
                disableCancellationToken?.Dispose();
                disableCancellationToken = new CancellationTokenSource();
                boosterAnimator.Play(boosterDisappearAnim, -1, 0f);
                await UniTask.Delay(Mathf.CeilToInt(boosterAnimator.GetCurrentAnimatorStateInfo(0).length * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, disableCancellationToken.Token);
                
                if (disableCancellationToken.IsCancellationRequested)
                    return;
                
                gameObject.SetActive(false);
            }
        }
    }
}