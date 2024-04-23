using HotPlay.PecanUI;
using HotPlay.QuickMath.Analytics;
using System.Collections.Generic;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PecanDailyUIInitializer : PecanUIInitializerBase
    {
        [Inject]
        private CurrencyDataController currencyDataController;
        
        public override void Setup()
        {
            Services.Events.DailyLoginRewardEventsHandler.ClaimReward += ClaimDailyReward;
        }

        public override void Terminate()
        {
            Services.Events.DailyLoginRewardEventsHandler.ClaimReward -= ClaimDailyReward;
        }
        
        private void ClaimDailyReward(int reward)
        {
            currencyDataController.Add(reward);

            int count = PecanServices.Instance.DailyLoginManager.GetCurrentRewardCount();
            int lastWeekCount = PecanServices.Instance.DailyLoginManager.LastWeekClaimedCount;
            string itemId = $"day_{(count == 1 ? lastWeekCount : count - 1)}";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { AnalyticsParams.ResourceFlowType, AnalyticsParams.ResourceGainType },
                { AnalyticsParams.CurrencyType, AnalyticsParams.Coin },
                { AnalyticsParams.Amount, reward},
                { AnalyticsParams.ItemType, AnalyticsParams.Daily },
                { AnalyticsParams.ItemID, itemId }
            };
            AnalyticsServices.Instance.LogResourceEvent(parameters);
        }
    }
}