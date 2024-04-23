using System.Collections.Generic;
using GameAnalyticsSDK;

namespace HotPlay.QuickMath.Analytics
{
    public class AnalyticsServices : Utilities.Singleton<AnalyticsServices>
    {
        private List<IAnalyticsProvider> analyticsProviders = new List<IAnalyticsProvider>();

        public void Init()
        {
            analyticsProviders.Add(new GameAnalyticsProvider() as IAnalyticsProvider);
        }

        public void LogEvent(string eventName, float value, Dictionary<string, object> data = null)
        {
            foreach(var provider in analyticsProviders)
            {
                provider.LogEvent(eventName, value, data);
            }
        }

        public void LogProgressionEvent(Dictionary<string, object> data)
        {
            foreach(var provider in analyticsProviders)
            {
                provider.LogProgressionEvent(data);
            }
        }

        public void LogResourceEvent(Dictionary<string, object> data)
        {
            foreach(var provider in analyticsProviders)
            {
                provider.LogResourceEvent(data);
            }
        }
    }
}