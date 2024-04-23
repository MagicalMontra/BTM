using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HotPlay.QuickMath.Analytics
{
    [CreateAssetMenu(fileName = "AnalyticConfigs", menuName = "HotPlay/Analytic Configs")]
    public class AnalyticsConfigs : ScriptableObject
    {
        [Serializable]
        public class PlatformConfig
        {
            public RuntimePlatform Platform;
            public string GameKey;
            public string SecretKey;
        }

        [SerializeField]
        private PlatformConfig[] developmentConfigs;
        public PlatformConfig[] DevelopmentConfigs => developmentConfigs;

        [SerializeField]
        private PlatformConfig[] productionConfigs;
        public PlatformConfig[] ProductionConfigs => productionConfigs;

        public PlatformConfig GetConfigs()
        {
            var platform = Application.platform;

#if UNITY_EDITOR
            platform = RuntimePlatform.Android;
#endif

#if PRODUCTION
            var config = productionConfigs.FirstOrDefault(x => x.Platform == platform);
#else
            var config = developmentConfigs.FirstOrDefault(x => x.Platform == platform);
#endif
            Debug.Assert(config != null, $"Not found config for platform [{platform}]");
            return config;
        }
    }
}