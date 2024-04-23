using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.Gameplay;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class GameResultEventsHandler : MonoBehaviour
    {
        [Obsolete("deprecated, in case of binding return to main menu game logic, please use GameplayEventsHandler.BackToMainMenu.", true)]
        public event Action HomeButtonClicked;
        public event Action LeaderboardButtonClicked;
        
        [Obsolete("deprecated, in case of binding restart to main menu game logic, please use GameplayEventsHandler.Restart.", true)]
        public event Action PlayAgainButtonClicked;

        [SerializeField]
        private ResultDialog resultDialog;

        private int score;
        
        private SignalStream gameResultSignalStream;
        private SignalReceiver gameResultSignalReceiver;
        
        private IAnalyticEvent<DesignEventData<int>, int> durationEvent;
        private IAnalyticEvent<DesignEventData<int>, int> leaderboardEvent;

        private void Start()
        {
            gameResultSignalStream = SignalStream.Get("Gameplay", "Result");
            gameResultSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
            gameResultSignalStream.ConnectReceiver(gameResultSignalReceiver);
        }

        private void OnSignal(Signal signal)
        {
            var data = signal.GetValueUnsafe<GameResultData>();
            score = data.Score;
            PecanServices.Instance.SessionTimer.StopSession();
            durationEvent ??= new IntAnalyticDesignEvent("gameOver:duration:score");
            PecanServices.Instance.Analytic.TryLog(PecanServices.Instance.SessionTimer.Seconds, durationEvent);
            resultDialog.Setup(data);
        }

        public void InvokeLeaderboardButtonClicked()
        {
            leaderboardEvent ??= new IntAnalyticDesignEvent("leaderboard:gameOver:view");
            PecanServices.Instance.Analytic.TryLog(score, leaderboardEvent);
            LeaderboardButtonClicked?.Invoke();
        }
    }
}
