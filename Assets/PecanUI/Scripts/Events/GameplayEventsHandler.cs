using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.Gameplay;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class GameplayEventsHandler : MonoBehaviour
    {
        public event Action Play;
        public event Action Restart;
        public event Action BackToMainMenu;
        
        [Obsolete("deprecated, in case of binding pause game logic, please use PauseEventHandler.Pause.", true)]
        public event Action PauseButtonClicked;
        public event Action TutorialButtonClicked;
        
        private int restartCount = 0;
        
        private GameResultData gameResultData;
        
        private SignalStream playSignalStream;
        private SignalReceiver playSignalReceiver;

        private SignalStream restartSignalStream;
        private SignalReceiver restartSignalReceiver;

        private SignalStream backToMainMenuSignalStream;
        private SignalReceiver backToMainMenuSignalReceiver;
        
        private SignalStream gameResultSignalStream;
        private SignalReceiver gameResultSignalReceiver;
        
        private IAnalyticEvent<DesignEventData<int>, int> playEvent;
        private IAnalyticEvent<DesignEventData<int>, int> restartEvent;
        private IAnalyticEvent<DesignEventData<int>, int> playAgainEvent;

        private void Start()
        {
            playSignalStream = SignalStream.Get("Gameplay", "StartResponse");
            playSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnPlaySignal);
            playSignalStream.ConnectReceiver(playSignalReceiver);

            restartSignalStream = SignalStream.Get("Gameplay", "RestartResponse");
            restartSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnRestartSignal);
            restartSignalStream.ConnectReceiver(restartSignalReceiver);

            backToMainMenuSignalStream = SignalStream.Get("MainMenu", "Response");
            backToMainMenuSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnBackToMainMenu);
            backToMainMenuSignalStream.ConnectReceiver(backToMainMenuSignalReceiver);
            
            gameResultSignalStream = SignalStream.Get("Gameplay", "Result");
            gameResultSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnGameOver);
            gameResultSignalStream.ConnectReceiver(gameResultSignalReceiver);
        }

        public void InvokeTutorialButtonClicked()
        {
            TutorialButtonClicked?.Invoke();
        }

        private void OnPlaySignal(Signal signal)
        {
            gameResultData = null;
            PecanServices.Instance.SessionTimer.StartSession();
            playEvent ??= new IntAnalyticDesignEvent("gameOver:playFromHome:score");
            PecanServices.Instance.Analytic.TryLog(PecanServices.Instance.HighScore, playEvent);
            Play?.Invoke();
        }

        private void OnRestartSignal(Signal signal)
        {
            PecanServices.Instance.SessionTimer.StartSession();
            LogRestartEvent();
            Restart?.Invoke();
            gameResultData = null;
        }

        private void LogRestartEvent()
        {
            restartEvent ??= new IntAnalyticDesignEvent("gameOver:restart:count");
            playAgainEvent ??= new IntAnalyticDesignEvent("gameOver:playAgain:score");
            var @event = gameResultData != null ? playAgainEvent : restartEvent;

            if (gameResultData == null)
                restartCount++;
            
            PecanServices.Instance.Analytic.TryLog(gameResultData?.Score ?? restartCount, @event);
        }

        private void OnGameOver(Signal signal)
        {
            gameResultData = signal.GetValueUnsafe<GameResultData>();
        }

        private void OnBackToMainMenu(Signal signal)
        {
            gameResultData = null;
            BackToMainMenu?.Invoke();
        }
    }
}
