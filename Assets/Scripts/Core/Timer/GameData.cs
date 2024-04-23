using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Data/Create GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        public AnimationCurve AnswerTimeCurve => answerTimeCurve;
        
        public AnimationCurve AnswerTimeRecoverCurve => answerTimeRecoverCurve;

        [SerializeField]
        private AnimationCurve answerTimeCurve;
        
        [SerializeField]
        private AnimationCurve answerTimeRecoverCurve;
    }
}