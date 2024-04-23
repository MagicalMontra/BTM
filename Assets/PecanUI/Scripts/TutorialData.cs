using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    [Serializable]
    public struct TutorialData
    {
        public Sprite TutorialSprite;
        public string TutorialKey;

        public TutorialData(Sprite sprite, string tutorialKey)
        {
            TutorialSprite = sprite;
            TutorialKey = tutorialKey;
        }
    }

    [Serializable]
    public struct TutorialPlatformData
    {
        public TutorialControlType ControlType;

        public TutorialData Data;

        public TutorialPlatformData(TutorialControlType controlType, TutorialData data)
        {
            ControlType = controlType;
            Data = data;
        }
    }

    [Serializable]
    public class TutorialPageData
    {
        public string Group;

        [Tooltip("Fallback data if not override by platform")]
        public TutorialData DefaultData;

        [Tooltip("Data look up base on current platform")]
        public TutorialPlatformData[] PlatformData;

        public TutorialPageData(TutorialData defaultData, TutorialPlatformData[] platformData, string group = null)
        {
            DefaultData = defaultData;
            PlatformData = platformData;
            Group = group;
        }

        /// <summary>
        /// Get tutorial data base on current platform. Fallback to default data if not found platform override
        /// </summary>
        public TutorialData GetCurrentPlatformTutorial()
        {
            TutorialControlType targetControlType = TutorialControlType.Mouse;

#if PECAN_NAVIGATOR
            targetControlType = TutorialControlType.Navigator;
#elif UNITY_WEBGL
            if(Application.isMobilePlatform)
            {
                targetControlType = TutorialControlType.Touch;
            }
            else
            {
                targetControlType = TutorialControlType.Mouse;
            }
#else
            targetControlType = TutorialControlType.Touch;
#endif

            if (Array.Exists(PlatformData, x => x.ControlType == targetControlType))
            {
                return Array.Find(PlatformData, x => x.ControlType == targetControlType).Data;
            }

            return DefaultData;
        }
    }

    public enum TutorialControlType
    {
        Mouse = 0,
        Touch = 1,
        Navigator = 2
    }
}
