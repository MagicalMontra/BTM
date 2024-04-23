using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int numberA = Random.Range(5, 10);
            int numberB = Random.Range(5, 10);
            int result = numberA + numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}