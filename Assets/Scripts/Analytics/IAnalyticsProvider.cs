using System.Collections.Generic;

namespace HotPlay.QuickMath.Analytics
{
    public interface IAnalyticsProvider
    {
        public void Init();
        public void LogEvent(string eventName, float value, Dictionary<string, object> data = null);
        public void LogProgressionEvent(Dictionary<string, object> data);
        public void LogResourceEvent(Dictionary<string, object> data);
    }
}