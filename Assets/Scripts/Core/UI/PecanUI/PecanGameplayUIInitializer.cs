using UnityEngine.Assertions;
using Zenject;
using HotPlay.QuickMath.Analytics;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using HotPlay.PecanUI.Leaderboard;

namespace HotPlay.BoosterMath.Core
{
    public class PecanGameplayUIInitializer : PecanUIInitializerBase
    {
        [Inject]
        private SignalBus signalBus;

        [Inject]
        private GameSessionController sessionController;
        
        [Inject]
        private SoundData soundData;

        public override void Setup()
        {
            Assert.IsNotNull(Services);
            Services.SetGetHighScoreFunction(GetHighScore);
            Services.SetIsNewHighScoreFunction(GetNewHighScore);
            Services.Events.GameplayEventsHandler.Play += Play;
            Services.Events.GameplayEventsHandler.Restart += Restart;
            Services.Events.GameplayEventsHandler.TutorialButtonClicked += Tutorial;
        }

        public override void Terminate()
        {
            Services.Events.GameplayEventsHandler.Play -= Play;
            Services.Events.GameplayEventsHandler.Restart -= Restart;
            Services.Events.GameplayEventsHandler.TutorialButtonClicked -= Tutorial;
        }

        private int GetHighScore()
        {
            return sessionController.CurrentHighScore;
        }
        
        private bool GetNewHighScore(int score)
        {
            return score > sessionController.CurrentHighScore;
        }

        private void Play()
        {
            if (Services.GetDialog<LeaderboardDialog>().State is VisibilityState.Visible or VisibilityState.IsShowing)
                return;
            
            Services.SoundManager.SoundPlayer.StopBGM();
            Services.SoundManager.SoundPlayer.PlayBGM(soundData.GameplayBGM);
            signalBus?.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Initialization));
            AnalyticsHelper.StartGame();
        }

        private void Restart()
        {
            Services.SoundManager.SoundPlayer.PlayBGM(soundData.GameplayBGM);
            AnalyticsHelper.Restart();
        }

        private void Tutorial()
        {
            AnalyticsHelper.LogTutorialStartEvent("helpBtn");
        }
    }
}