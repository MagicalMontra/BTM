using System;
using System.Collections.Generic;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using HotPlay.QuickMath.Analytics;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class GameSessionController
    {
        public Action OnScoreIncreased;

        public Func<float> ScoreMultiplier;

        public int CurrentCoin { get; private set; }

        public int CurrentStage { get; private set; }

        public int CurrentScore { get; private set; }

        public int CurrentHighScore { get; private set; }

        private readonly PecanServices services;

        private readonly GameModeController gameModeController;

        private readonly CurrencyDataController currencyDataController;

        private const string highScoreKey = "HighScore";

        public GameSessionController(PecanServices services, GameModeController gameModeController, CurrencyDataController currencyDataController)
        {
            this.services = services;
            this.gameModeController = gameModeController;
            this.currencyDataController = currencyDataController;
            CurrentHighScore = PlayerPrefs.HasKey(highScoreKey) ? PlayerPrefs.GetInt(highScoreKey) : 0;
        }

        public void Start()
        {
            CurrentCoin = 0;
            CurrentStage = 1;
            CurrentScore = 0;
            services.Events.InitGameplayCurrency(CurrentCoin);
            services.GetCustomGamePlayPanel<GameplayUI>().UpdateScore(CurrentScore);
        }

        public void IncreaseStage()
        {
            CurrentStage++;
        }

        public void IncreaseCoin()
        {
            CurrentCoin++;
            services.Events.UpdateGameplayCurrency(CurrentCoin);
        }

        public void End()
        {
            var isHighScore = CurrentScore > CurrentHighScore;

            if (isHighScore)
            {
                CurrentHighScore = CurrentScore;
                PlayerPrefs.SetInt(highScoreKey, CurrentScore);
                PlayerPrefs.Save();
            }

            currencyDataController.Add(CurrentCoin);
            services.GetCustomGamePlayPanel<GameplayUI>().UpdateScore("");
            services.Signals.SendResultSignal(new GameResultData(isHighScore, CurrentScore, CurrentHighScore, CurrentCoin));

            LogGameplayCoinsEvent();
        }

        public void IncreaseScore()
        {
            var multiplier = ScoreMultiplier?.Invoke();
            CurrentScore += Mathf.CeilToInt(multiplier.HasValue ? gameModeController.CurrentGameMode.Settings.ScoreModifier * multiplier.Value : gameModeController.CurrentGameMode.Settings.ScoreModifier);
            services.GetCustomGamePlayPanel<GameplayUI>().UpdateScore(CurrentScore);
        }

        #if CHEAT_ENABLED
        public void DebugIncreaseScore(int score)
        {
            CurrentScore += score;
            services.GetCustomGamePlayPanel<GameplayUI>().UpdateScore(CurrentScore);
        }
        #endif

        private void LogGameplayCoinsEvent()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { AnalyticsParams.ResourceFlowType, AnalyticsParams.ResourceGainType },
                { AnalyticsParams.CurrencyType, AnalyticsParams.Coin },
                { AnalyticsParams.Amount, CurrentCoin },
                { AnalyticsParams.ItemType, AnalyticsParams.Coin },
                { AnalyticsParams.ItemID, "-" }
            };

            AnalyticsServices.Instance.LogResourceEvent(parameters);
        }
    }
}