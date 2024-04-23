using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMultiplyYYZZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            var r = Random.Range(10, maxNumber + 1);
            var max = Mathf.FloorToInt(999 / r);
            
            if (max > 50)
                max = 50;
            
            int numberA = r;
            int numberB = Random.Range(10, max);
            int result = numberA * numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Multiply);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}