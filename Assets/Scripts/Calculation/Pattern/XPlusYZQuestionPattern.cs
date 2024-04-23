using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int remain = maxNumber;
            int numberA = Random.Range(1, maxNumber - 1);
            remain -= numberA;
            int numberB = Random.Range(2, remain + 1);
            int result = numberA + numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}