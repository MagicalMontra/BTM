using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using HotPlay.DailyLogin;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.DailyLogin;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class DailyLoginRewardEventsHandler : MonoBehaviour
    {
        public event Action<int> ClaimReward;

        /// <summary>
        /// bool: is last coin
        /// </summary>
        public event Action<bool> CoinAnimReached;
        public event Action ClaimButtonClicked;

        [SerializeField]
        private DailyLoginRewardDialog dailyLoginRewardDialog;

        private SignalReceiver rewardSignalReceiver;
        private SignalStream rewardSignalStream;

        private void Start()
        {
            rewardSignalStream = SignalStream.Get("DailyLogin", "Reward");
            rewardSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
            rewardSignalStream.ConnectReceiver(rewardSignalReceiver);
        }

        private void OnSignal(Signal signal)
        {
            var claimRewardData = signal.GetValueUnsafe<ClaimRewardData>();
            dailyLoginRewardDialog.Setup(claimRewardData).Forget();
        }

        public void InvokeClaimReward(int reward)
        {
            ClaimReward?.Invoke(reward);
        }

        public void InvokeCoinAnimReached(bool isLastCoin)
        {
            CoinAnimReached?.Invoke(isLastCoin);
        }

        public void InvokeClaimButtonClicked()
        {
            ClaimButtonClicked?.Invoke();
        }
    }
}
