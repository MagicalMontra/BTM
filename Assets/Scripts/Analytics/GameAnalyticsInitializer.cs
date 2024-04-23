using UnityEngine;
using HotPlay.QuickMath.Analytics;

namespace HotPlay.QuickMath
{
    public class GameAnalyticsInitializer : MonoBehaviour
    {
        private void Awake()
        {
            AnalyticsServices.Instance.Init();
        }
    }
}

