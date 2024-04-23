using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class EventsHandler : MonoBehaviour
    {
        /// <summary>
        /// int: player currency
        /// bool: trigger currency animation
        /// </summary>
        public event Action<int, bool> SoftCurrencyUpdate;

        /// <summary>
        /// int: game play currency
        /// bool: trigger currency animation
        /// </summary>
        public event Action<int, bool> GameplayCurrencyUpdate;

        /// <summary>
        /// TopBarType: target top bar
        /// boo: play transition
        /// </summary>
        public event Action<TopBarType, bool> TopBarChange;

        public event Action<bool> EnableBGM;
        public event Action<bool> EnableSFX;

        public event Action<string> PlayerIDUpdate;

        [SerializeField]
        private MainMenuEventsHandler mainMenuEventsHandler;
        public MainMenuEventsHandler MainMenuEventsHandler => mainMenuEventsHandler;

        [SerializeField]
        private ShopEventsHandler shopEventsHandler;
        public ShopEventsHandler ShopEventHandler => shopEventsHandler;

        [SerializeField]
        private ItemPreviewEventsHandler itemPreviewEventsHandler;
        public ItemPreviewEventsHandler ItemPreviewEventsHandler => itemPreviewEventsHandler;

        [SerializeField]
        private SettingsEventHandler settingsEventHandler;
        public SettingsEventHandler SettingsEventHandler => settingsEventHandler;

        [SerializeField]
        private LeaderboardEventsHandler leaderboardEventHandler;
        public LeaderboardEventsHandler LeaderboardEventsHandler => leaderboardEventHandler;

        [SerializeField]
        private DailyLoginEventsHandler dailyLoginEventsHandler;
        public DailyLoginEventsHandler DailyLoginEventsHandler => dailyLoginEventsHandler;

        [SerializeField]
        private DailyLoginRewardEventsHandler dailyLoginRewardEventsHandler;
        public DailyLoginRewardEventsHandler DailyLoginRewardEventsHandler => dailyLoginRewardEventsHandler;

        [SerializeField]
        private TutorialEventsHandler tutorialEventsHandler;
        public TutorialEventsHandler TutorialEventsHandler => tutorialEventsHandler;

        [SerializeField]
        private GameResultEventsHandler gameResultEventsHandler;
        public GameResultEventsHandler GameResultEventsHandler => gameResultEventsHandler;

        [SerializeField]
        private PauseEventsHandler pauseEventsHandler;
        public PauseEventsHandler PauseEventsHandler => pauseEventsHandler;

        [SerializeField]
        private GameplayEventsHandler gameplayEventsHandler;
        public GameplayEventsHandler GameplayEventsHandler => gameplayEventsHandler;

        // Need to call this when start the game to let PecanUI update properly
        public void InitSoftCurrency(int playerSoftCurrency)
        {
            SoftCurrencyUpdate?.Invoke(playerSoftCurrency, false);
        }

        public void UpdateSoftCurrency(int playerSoftCurrency)
        {
            SoftCurrencyUpdate?.Invoke(playerSoftCurrency, true);
        }

        // Need to call this when start gameplay to let PecanUI update properly
        public void InitGameplayCurrency(int currency)
        {
            GameplayCurrencyUpdate?.Invoke(currency, false);
        }

        public void UpdateGameplayCurrency(int currency)
        {
            GameplayCurrencyUpdate?.Invoke(currency, true);
        }

        public void ChangeTopBar(TopBarType type, bool transition)
        {
            TopBarChange?.Invoke(type, transition);
        }

        // Need to call this when start the game to let PecanUI update properly
        public void InitSoundsStatus(bool enableBGM, bool enableSFX)
        {
            UpdateBGMStatus(enableBGM);
            UpdateSFXStatus(enableSFX);
        }

        public void UpdatePlayerID(string id)
        {
            PlayerIDUpdate?.Invoke(id);
        }

        public void UpdateBGMStatus(bool status)
        {
            EnableBGM?.Invoke(status);
        }

        public void UpdateSFXStatus(bool status)
        {
            EnableSFX?.Invoke(status);
        }
    }
}
