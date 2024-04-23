using Spine.Unity;
using UnityEngine;

namespace HotPlay.Utilities
{
    public static class SpineAnimationHelper
    {
        private const int millisecondMultiplier = 1000;
        
        public static int GetDurationMS(this AnimationReferenceAsset clip)
        {
            return Mathf.CeilToInt(clip.Animation.Duration * millisecondMultiplier);
        }
    }

    public static class UniTaskDelayHelper
    {
        private const int millisecondMultiplier = 1000;
        
        public static int GetDurationMS(this float seconds)
        {
            return Mathf.CeilToInt(seconds * millisecondMultiplier);
        }
    }
}