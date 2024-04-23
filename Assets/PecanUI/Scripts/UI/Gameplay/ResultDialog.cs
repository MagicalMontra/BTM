
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using HotPlay.PecanUI.Events;
using HotPlay.PecanUI.Leaderboard;
using HotPlay.Utilities;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.Gameplay
{
    public class ResultDialog : BaseDialog
    {
        [SerializeField]
        private UIContainer newHighScoreIcon;
        
        [SerializeField]
        private SkeletonGraphic newHighScoreAnimation;

        [SerializeField]
        private AnimationReferenceAsset newHighScoreRef;

        [SerializeField]
        private TextMeshProUGUI currentScore;

        [SerializeField]
        private TextMeshProUGUI currencyEarnedText;

        [SerializeField]
        private UIButton homeButton;

        [SerializeField]
        private UIButton leaderboardButton;

        [SerializeField]
        private UIButton playAgainButton;

        private GameResultData resultData;

        private bool isNewHighScorePlaying;
        private bool leaderboardOpened;

        private void Start()
        {
            leaderboardButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnLeaderboardButtonClicked);
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            newHighScoreIcon.InstantHide();
            playAgainButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnPlayAgainButtonClicked);
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            playAgainButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnPlayAgainButtonClicked);
        }

        protected override void OnVisible()
        {
            base.OnVisible();

            if (resultData != null && resultData.IsNewHighScore)
            {
                PlayNewHighScoreAnimation().Forget();
            }
            else
            {
                newHighScoreIcon.InstantHide();
            }

            TryTriggerLeaderboard();
        }

        private async UniTaskVoid PlayNewHighScoreAnimation()
        {
            newHighScoreIcon.Show();
            newHighScoreAnimation.AnimationState.SetAnimation(0, newHighScoreRef, false);
            isNewHighScorePlaying = true;
            await UniTask.Delay(Mathf.CeilToInt(newHighScoreRef.Animation.Duration * 1000));
            isNewHighScorePlaying = false;
            await UniTask.Yield();
        }

        private void TryTriggerLeaderboard()
        {
            if (resultData == null || leaderboardOpened)
            {
                return;
            }

            switch (PecanServices.Instance.Configs.ResultLeaderboardType)
            {
                case ResultLeaderboardType.Always:
                    StartCoroutine(ShowLeaderboardRoutine());
                    break;
                case ResultLeaderboardType.BestScore:
                    if (resultData.IsNewHighScore)
                    {
                        StartCoroutine(ShowLeaderboardRoutine());
                    }
                    break;
                case ResultLeaderboardType.Never:
                    LeaderboardSignalData signalData = new LeaderboardSignalData(resultData.PrevScore, resultData.Score, LeaderboardDialogOpenType.Auto);
                    PecanServices.Instance.Events.LeaderboardEventsHandler.SetSignalData(signalData);
                    break;
            }
        }

        private IEnumerator ShowLeaderboardRoutine()
        {
            leaderboardOpened = true;
            homeButton.enabled = false;
            leaderboardButton.enabled = false;
            playAgainButton.enabled = false;

            yield return new WaitWhile(() => isNewHighScorePlaying);
            yield return new WaitForSeconds(1.5f);

            homeButton.enabled = true;
            leaderboardButton.enabled = true;
            playAgainButton.enabled = true;

            LeaderboardSignalData signalData = new LeaderboardSignalData(
                resultData.PrevScore, 
                resultData.Score, 
                LeaderboardDialogOpenType.Auto
            );
            Signal.Send("MainMenu", "Leaderboard", signalData);
        }

        public void Setup(GameResultData data)
        {
            leaderboardOpened = false;
            resultData = data;
            currentScore.SetText(data.Score.ToString());
            currencyEarnedText.SetText(data.CurrencyEarned.ToString());
        }

        private void OnLeaderboardButtonClicked()
        {
            PecanServices.Instance.Events.GameResultEventsHandler.InvokeLeaderboardButtonClicked();
        }

        private void OnPlayAgainButtonClicked()
        {
            playAgainButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnPlayAgainButtonClicked);
        }
    }

    [Serializable]
    public class GameResultData
    {
        public bool IsNewHighScore;
        public int Score;
        public int CurrencyEarned;
        public int PrevScore;

        public GameResultData(bool isNewHighScore, int score, int prevScore, int currencyEarned)
        {
            IsNewHighScore = isNewHighScore;
            Score = score;
            PrevScore = prevScore;
            CurrencyEarned = currencyEarned;
        }
    }
}
