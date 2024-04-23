using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Analytic;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class MainMenuEventsHandler : MonoBehaviour
    {
        private const string signalCategory = "MainMenu";

        public event Action PlayButtonClicked;
        public event Action ShopButtonClicked;
        public event Action GiftButtonClicked;
        public event Action LeaderboardButtonClicked;
        public event Action SettingButtonClicked;

        private SignalStream shopButtonSignalStream;
        private SignalReceiver shopButtonSignalReceiver;
        private SignalStream giftButtonSignalStream;
        private SignalReceiver giftButtonSignalReceiver;
        private SignalStream leaderboardButtonSignalStream;
        private SignalReceiver leaderboardButtonSignalReceiver;
        private SignalStream settingButtonSignalStream;
        private SignalReceiver settingButtonSignalReceiver;

        private IAnalyticEvent<DesignEventData<int>, int> leaderboardEvent;
        
        private void Start()
        {
            shopButtonSignalStream = SignalStream.Get(signalCategory, "Shop");
            shopButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnShopButtonSignal);
            shopButtonSignalStream.ConnectReceiver(shopButtonSignalReceiver);

            giftButtonSignalStream = SignalStream.Get(signalCategory, "Gift");
            giftButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnGiftButtonSignal);
            giftButtonSignalStream.ConnectReceiver(giftButtonSignalReceiver);

            leaderboardButtonSignalStream = SignalStream.Get(signalCategory, "Leaderboard");
            leaderboardButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnLeaderboardButtonSignal);
            leaderboardButtonSignalStream.ConnectReceiver(leaderboardButtonSignalReceiver);

            settingButtonSignalStream = SignalStream.Get(signalCategory, "Settings");
            settingButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSettingButtonSignal);
            settingButtonSignalStream.ConnectReceiver(settingButtonSignalReceiver);
        }

        public void InvokePlayButtonClicked()
        {
            PlayButtonClicked?.Invoke();
        }

        private void OnShopButtonSignal(Signal signal)
        {
            ShopButtonClicked?.Invoke();
        }

        private void OnGiftButtonSignal(Signal signal)
        {
            GiftButtonClicked?.Invoke();
        }

        private void OnLeaderboardButtonSignal(Signal signal)
        {
            leaderboardEvent ??= new IntAnalyticDesignEvent("leaderboard:home:view");
            PecanServices.Instance.Analytic.TryLog(PecanServices.Instance.HighScore, leaderboardEvent);
            LeaderboardButtonClicked?.Invoke();
        }

        private void OnSettingButtonSignal(Signal signal)
        {
            SettingButtonClicked?.Invoke();
        }
    }
}
