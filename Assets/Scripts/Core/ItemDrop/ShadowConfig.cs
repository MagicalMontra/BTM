using System;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    [Serializable]
    public class ShadowConfig
    {
        [SerializeField]
        private float minInterestDistance;
        public float MinInterestDistance => minInterestDistance;

        [SerializeField]
        private float maxInterestDistance;
        public float MaxInterestDistance => maxInterestDistance;

        [SerializeField]
        private float minScale;
        public float MinScaling => minScale;

        [SerializeReference]
        private float maxScale;
        public float MaxScaling => maxScale;

        [SerializeField]
        private float minAlpha;
        public float MinAlpha => minAlpha;

        [SerializeField]
        private float maxAlpha;
        public float MaxAlpha => maxAlpha;
    }
}