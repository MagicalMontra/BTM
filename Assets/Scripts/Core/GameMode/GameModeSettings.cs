using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HotPlay.BoosterMath.Core.UI
{
    [Serializable]
    public class GameModeSettings
    {
        public int MaxStage => maxStage;
        
        public int ScoreModifier => scoreModifier;

        public GameModeEnum GameModeEnum => gameModeEnum;

        public int MaxAnswerRange => maxAnswerRange;
        
        public int MinFakeAnswerRange => minFakeAnswerRange;

        public int MaxFakeAnswerRange => maxFakeAnswerRange;
        
        public int MinCoinAmount => minCoinAmount;

        public int MaxCoinAmount => maxCoinAmount;

        public float CoinMultiplier => coinMultiplier;

        public float HeartDropRate => heartDropRate;

        public float BoosterModifier => boosterModifier;
        
        public float CoinDropRate => coinDropRate;
        
        public AnimationCurve EnemyCurve => enemyCurve;
        
        public AnimationCurve ItemDropCurve => itemDropCurve;

        [SerializeField]
        private GameModeEnum gameModeEnum;

        [SerializeField]
        private int minFakeAnswerRange;
        
        [SerializeField]
        private int maxFakeAnswerRange;
        
        [SerializeField]
        private int maxAnswerRange;
        
        [SerializeField]
        public int maxStage = int.MaxValue;

        [SerializeField]
        private int minCoinAmount = 1;
        
        [SerializeField]
        private int maxCoinAmount = 3;
        
        [SerializeField]
        private int scoreModifier = 100;

        [SerializeField]
        private float coinMultiplier = 1f;

        [SerializeField]
        private float coinDropRate = 1f;

        [SerializeField]
        private float heartDropRate = 0.15f;

        [SerializeField]
        private float boosterModifier = 0.25f;

        [SerializeField]
        private AnimationCurve enemyCurve;
        
        [SerializeField]
        private AnimationCurve itemDropCurve;
    }
}