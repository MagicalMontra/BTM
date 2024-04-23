using System;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{

    public class LeaderboardEventsHandler : MonoBehaviour
    {
        public event Action CloseButtonClicked;

        private SignalStream leaderboardSignalStream;
        private SignalReceiver leaderboardSignalReceiver;
        public event Action<LeaderboardDialogOpenType> WhenLeaderboardDialogOpened;

        public LeaderboardSignalData SignalData { get; private set; }
        public bool HasSignalData => SignalData != null;

        private void Start()
        {
            leaderboardSignalStream = SignalStream.Get("MainMenu", "Leaderboard");
            leaderboardSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
            leaderboardSignalStream.ConnectReceiver(leaderboardSignalReceiver);
        }

        public void InvokeCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
        }

        public void ClearSignalData()
        {
            SetSignalData(null);
        }

        public void SetSignalData(LeaderboardSignalData signalData)
        {
            SignalData = signalData;
        }

       internal void RaiseLeaderboardDialogOpened(LeaderboardDialogOpenType openType)
        {
            WhenLeaderboardDialogOpened?.Invoke(openType);
        }

        private void OnSignal(Signal signal)
        {
            if (signal.hasValue)
            {
                var signalValue = signal.valueAsObject;
                SignalData = signalValue is string value
                    ? new LeaderboardSignalData(0, 0, LeaderboardDialogOpenTypeExtension.Parse(value))
                    : signal.GetValueUnsafe<LeaderboardSignalData>();
            }
        }
    }
}
