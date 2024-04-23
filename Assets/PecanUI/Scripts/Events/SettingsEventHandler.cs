using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Analytic;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class SettingsEventHandler : MonoBehaviour
    {
        private const string signalCategory = "Settings";

        public event Action<bool> EnableBGM;
        public event Action<bool> EnableSFX;
        public event Action FacebookButtonClicked;
        public event Action TwitterButtonClicked;
        public event Action SupportButtonClicked;
        public event Action PrivacyPolicyButtonClicked;
        public event Action<string> LanguageChanged;
        public event Action CloseButtonClicked;

        private SignalStream facebookButtonSignalStream;
        private SignalReceiver facebookButtonSignalReceiver;
        private SignalStream twitterButtonSignalStream;
        private SignalReceiver twitterButtonSignalReceiver;
        private SignalStream supportButtonSignalStream;
        private SignalReceiver supportButtonSignalReceiver;
        private SignalStream privacyPolicyButtonSignalStream;
        private SignalReceiver privacyPolicyButtonSignalReceiver;
        private SignalStream enableBGMSignalStream;
        private SignalReceiver enableBGMSignalReceiver;
        private SignalStream enableSFXSignalStream;
        private SignalReceiver enableSFXSignalReceiver;
        private SignalStream languageChangeSignalStream;
        private SignalReceiver languageChangeSignalReceiver;

        private IAnalyticEvent<DesignEventData> languageEvent;
        private IAnalyticEvent<DesignEventData<string>, string> sfxEvent;
        private IAnalyticEvent<DesignEventData<string>, string> musicEvent;
        
        private void Start()
        {
            facebookButtonSignalStream = SignalStream.Get(signalCategory, "Facebook");
            facebookButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnFacebookButtonSignal);
            facebookButtonSignalStream.ConnectReceiver(facebookButtonSignalReceiver);

            twitterButtonSignalStream = SignalStream.Get(signalCategory, "Twitter");
            twitterButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnTwitterButtonSignal);
            twitterButtonSignalStream.ConnectReceiver(twitterButtonSignalReceiver);

            supportButtonSignalStream = SignalStream.Get(signalCategory, "Support");
            supportButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSupportButtonSignal);
            supportButtonSignalStream.ConnectReceiver(supportButtonSignalReceiver);

            privacyPolicyButtonSignalStream = SignalStream.Get(signalCategory, "PrivacyPolicy");
            privacyPolicyButtonSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnPrivacyPolicyButtonSignal);
            privacyPolicyButtonSignalStream.ConnectReceiver(supportButtonSignalReceiver);

            enableBGMSignalStream = SignalStream.Get(signalCategory, "EnableBGM");
            enableBGMSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnEnableBGM);
            enableBGMSignalStream.ConnectReceiver(enableBGMSignalReceiver);

            enableSFXSignalStream = SignalStream.Get(signalCategory, "EnableSFX");
            enableSFXSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnEnableSFX);
            enableSFXSignalStream.ConnectReceiver(enableSFXSignalReceiver);

            languageChangeSignalStream = SignalStream.Get(signalCategory, "Language");
            languageChangeSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnLanguageChange);
            languageChangeSignalStream.ConnectReceiver(languageChangeSignalReceiver);
        }
        
        public void InvokeCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
        }

        private void OnFacebookButtonSignal(Signal signal)
        {
            FacebookButtonClicked?.Invoke();
        }

        private void OnTwitterButtonSignal(Signal signal)
        {
            TwitterButtonClicked?.Invoke();
        }

        private void OnSupportButtonSignal(Signal signal)
        {
            SupportButtonClicked?.Invoke();
        }

        private void OnPrivacyPolicyButtonSignal(Signal signal)
        {
            PrivacyPolicyButtonClicked?.Invoke();
        }

        private void OnEnableBGM(Signal signal)
        {
            bool value = signal.GetValueUnsafe<bool>();
            var stringValue = value ? "1" : "0";
            var onOff = value ? "on" : "off";
            musicEvent = new StringAnalyticDesignEvent($"setting:audioMusic:{onOff}");
            PecanServices.Instance.Analytic.TryLog(stringValue, musicEvent);
            EnableBGM?.Invoke(value);
        }

        private void OnEnableSFX(Signal signal)
        {
            bool value = signal.GetValueUnsafe<bool>();
            var stringValue = value ? "1" : "0";
            var onOff = value ? "on" : "off";
            sfxEvent = new StringAnalyticDesignEvent($"setting:audioEffect:{onOff}");
            PecanServices.Instance.Analytic.TryLog(stringValue, sfxEvent);
            EnableSFX?.Invoke(value);
        }

        private void OnLanguageChange(Signal signal)
        {
            string value = signal.GetValueUnsafe<string>();
            languageEvent = new VoidAnalyticDesignEvent($"setting:language:{value}");
            PecanServices.Instance.Analytic.TryLog(languageEvent);
            LanguageChanged?.Invoke(value);
        }
    }
}
