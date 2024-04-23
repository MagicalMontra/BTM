using System.Collections.Generic;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace HotPlay.QuickMath.Analytics
{
    public class GameAnalyticsProvider : IAnalyticsProvider
    {
        public GameAnalyticsProvider()
        {
            Init();
        }

        public void Init()
        {
            var customConfig = Resources.Load<AnalyticsConfigs>("GameAnalytics/AnalyticConfigs").GetConfigs();
            var config = Resources.Load<Settings>("GameAnalytics/Settings");

            var index = config.Platforms.IndexOf(customConfig.Platform);
            config.UpdateGameKey(index, customConfig.GameKey);
            config.UpdateSecretKey(index, customConfig.SecretKey);

            GameAnalytics.Initialize();
        }

        public void LogEvent(string eventName, float value, Dictionary<string, object> data = null)
        {
            LogDesignEvent(eventName, value);
        }

        public void LogProgressionEvent(Dictionary<string, object> data)
        {
            if(!data.ContainsKey(AnalyticsParams.ProgressionStatus))
            {
                Debug.LogAssertion("Missing Progression status for GameAnalytics progression event");
                return;
            }

            GAProgressionStatus progressionStatus = GetGAProgressionStatus(data[AnalyticsParams.ProgressionStatus].ToString());

            string progressionName = string.Empty;
            if(data.ContainsKey(AnalyticsParams.ProgressionName01))
            {
                progressionName = data[AnalyticsParams.ProgressionName01].ToString();
            }

            int score = 0;
            if(data.ContainsKey(AnalyticsParams.Score))
            {
                score = (int)data[AnalyticsParams.Score];
            }

            GameAnalytics.NewProgressionEvent(progressionStatus, progressionName, score);
        }

        public void LogResourceEvent(Dictionary<string, object> data)
        {
            if(!data.ContainsKey(AnalyticsParams.ResourceFlowType))
            {
                Debug.LogAssertion("Missing Resource Type for GameAnalytics Resource event");
                return;
            }

            GAResourceFlowType resourceFlowType = GetGAResourceFlowType(data[AnalyticsParams.ResourceFlowType].ToString());
            string currency = string.Empty;
            if(data.ContainsKey(AnalyticsParams.CurrencyType))
            {
                currency = data[AnalyticsParams.CurrencyType].ToString();
            }

            float amount = 0;
            if(data.ContainsKey(AnalyticsParams.Amount))
            {
                amount = float.Parse(data[AnalyticsParams.Amount].ToString());
            }

            string itemType = string.Empty;
            if(data.ContainsKey(AnalyticsParams.ItemType))
            {
                itemType = data[AnalyticsParams.ItemType].ToString();
            }

            string itemId = string.Empty;
            if(data.ContainsKey(AnalyticsParams.ItemID))
            {
                itemId = data[AnalyticsParams.ItemID].ToString();
            }

            GameAnalytics.NewResourceEvent(resourceFlowType, currency, amount, itemType, itemId);
        }

        private void LogDesignEvent(string eventName, float value)
        {
            GameAnalytics.NewDesignEvent(eventName, value);
        }

        private GAResourceFlowType GetGAResourceFlowType(string value)
        {
            if(value == AnalyticsParams.ResourceGainType)
            {
                return GAResourceFlowType.Source;
            }
            else if(value == AnalyticsParams.ResourceSpendType)
            {
                return GAResourceFlowType.Sink;
            }
            else
            {
                return GAResourceFlowType.Undefined;
            }
        }

        private GAProgressionStatus GetGAProgressionStatus(string value)
        {
            if(value == AnalyticsParams.ProgressionStart)
            {
                return GAProgressionStatus.Start;
            }
            else if(value == AnalyticsParams.ProgressionComplete)
            {
                return GAProgressionStatus.Complete;
            }
            else if(value == AnalyticsParams.ProgressionFail)
            {
                return GAProgressionStatus.Fail;
            }
            else
            {
                return GAProgressionStatus.Undefined;
            }
        }
    }
}