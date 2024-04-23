using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using HotPlay.DailyLogin;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.DailyLogin
{
    public class DailyLoginRewardDialog : BaseDialog
    {
        [SerializeField]
        private TextMeshProUGUI amountLabel;
        
        [SerializeField]
        private SkeletonGraphic coinAnimation;

        [SerializeField]
        private AnimationReferenceAsset coinSAnimationRef;
        
        [SerializeField]
        private AnimationReferenceAsset coinMAnimationRef;
        
        [SerializeField]
        private AnimationReferenceAsset coinLAnimationRef;

        [SerializeField]
        private UIButton claimButton;

        [SerializeField]
        private Image[] coins;

        [SerializeField]
        private Transform coinInitPosition;
        
        [SerializeField]
        private int sAmountThreshold = 200;
        
        [SerializeField]
        private int mAmountThreshold = 680;
        
        [SerializeField]
        private int lAmountThreshold = 900;

        [SerializeField]
        private float moveCoinDuration;

        [SerializeField]
        private float coinDelay;
        
        [SerializeField]
        private string claimButtonSoundName = "UIButton/Play";
        
        [SerializeField]
        private string claimCoinSoundName = "UIButton/ClaimReward";

        [SerializeField]
        private Ease coinsAnimEase;

        private DailyLoginManager manager;

        private void Start()
        {
            manager = PecanServices.Instance.DailyLoginManager;
        }

        public void OnClickClaim()
        {
            PecanServices.Instance.Events.DailyLoginRewardEventsHandler.InvokeClaimButtonClicked();

            if (!manager.IsClaimed)
            {
                manager.Claim((reward) =>
                {
                    // coinAnimation.AnimationState.SetAnimation(0, MeasureRewardAmount(reward), false);
                    coinAnimation.freeze = false;
                    PecanServices.Instance.PecanSoundManager.PlayOnce(claimButtonSoundName);
                    PecanServices.Instance.Events.DailyLoginRewardEventsHandler.InvokeClaimReward(reward);
                    StartCoroutine(CoinAnimRoutine());
                });
            }
        }

        private AnimationReferenceAsset MeasureRewardAmount(int reward)
        {
            if (reward <= sAmountThreshold)
                return coinSAnimationRef;

            if (reward <= mAmountThreshold)
                return coinMAnimationRef;

            return reward <= lAmountThreshold ? coinLAnimationRef : coinSAnimationRef;
        }

        private IEnumerator CoinAnimRoutine()
        {
            claimButton.interactable = false;

            int completedCount = 0;
            foreach (Image coin in coins)
            {
                coin.gameObject.SetActive(true);
                coin.DOFade(1f, moveCoinDuration).SetEase(coinsAnimEase);
                var topBar = PecanServices.Instance.TopBar.GetTopBar<CommonTopBar>();
                var targetMoveCoin = topBar.PlayerCurrencyBar.transform.position;
                coin.transform.DOMove(targetMoveCoin, moveCoinDuration).SetEase(coinsAnimEase).OnComplete
                (
                    () =>
                    {
                        completedCount++;
                        coin.gameObject.SetActive(false);

                        if (completedCount == coins.Length)
                        {
                            PecanServices.Instance.Events.DailyLoginRewardEventsHandler.InvokeCoinAnimReached(true);
                            Signal.Send("DailyLogin", "Open");
                            return;
                        }

                        PecanServices.Instance.PecanSoundManager.PlayOnce(claimCoinSoundName);
                        PecanServices.Instance.Events.DailyLoginRewardEventsHandler.InvokeCoinAnimReached(false);
                    }
                );

                if (completedCount < coins.Length)
                {
                    yield return new WaitForSeconds(coinDelay);
                }
                
            }

            yield return null;
        }

        public async UniTaskVoid Setup(ClaimRewardData claimRewardData)
        {
            foreach (var coin in coins)
            {
                coin.gameObject.transform.position = coinInitPosition.transform.position;
            }
            
            claimButton.interactable = true;
            coinAnimation.AnimationState.ClearTrack(0);
            coinAnimation.AnimationState.SetAnimation(0, MeasureRewardAmount(claimRewardData.amount), false);
            await UniTask.DelayFrame(2);
            coinAnimation.freeze = true;
            amountLabel.text = claimRewardData.amount.ToString("N0");
            await UniTask.Yield();
        }
    }
}
