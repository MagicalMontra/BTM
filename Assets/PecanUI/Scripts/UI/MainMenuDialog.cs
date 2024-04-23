using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.goPlay.Internal.Config;
using HotPlay.goPlay.Services.Rewards;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class MainMenuDialog : BaseDialog
    {
        [SerializeField]
        private UIButton playButton;

        [SerializeField]
        private UIButton giftButton;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeReference]
        private Transform gameTitleHolder;

        private void Start()
        {
            var gameTitlePrefab = PecanServices.Instance.Configs.GameTitlePrefab;

            if (gameTitlePrefab != null && gameTitleHolder != null)
            {
                Instantiate(gameTitlePrefab, gameTitleHolder);
            }

            giftButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnGiftButtonClicked);
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            SetHighScoreText();

            if (PecanServices.Instance.DailyLoginManager.IsClaimed &&
                !PecanServices.Instance.DailyLoginManager.IsClaimCoupon &&
                GoPlaySettings.Instance.EnableRewardSystem &&
                GoPlayRewardService.Instance.CanReceiveReward())
            {
                GoPlayRewardService.Instance.ReceiveReward(response =>
                {
                    if (response.IsSuccess || !Application.isMobilePlatform)
                        PecanServices.Instance.DailyLoginManager.SetCouponNextClaim();
                });
            }
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            if (!PecanServices.Instance.DailyLoginManager?.IsClaimed ?? false)
            {
                Signal.Send("DailyLogin", "Open", "message");
                Debug.Log("Send daily login signal");
            }
            playButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnPlayButtonClicked);
            playButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnPlayButtonClicked);
        }

        private void OnGiftButtonClicked()
        {
            Signal.Send("MainMenu", "Gift");
        }

        private void OnPlayButtonClicked()
        {
            PecanServices.Instance.Events.MainMenuEventsHandler.InvokePlayButtonClicked();
            playButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnPlayButtonClicked);
        }

        private void SetHighScoreText()
        {
            scoreText.text = PecanServices.Instance.GetHighScore().ToString();
        }
    }
}
