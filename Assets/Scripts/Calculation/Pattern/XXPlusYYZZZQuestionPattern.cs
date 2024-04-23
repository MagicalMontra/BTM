using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXPlusYYZZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            var remain = 100;
            int numberA = Random.Range(51, remain);
            remain -= numberA;
            int numberB = Random.Range(remain, maxNumber + 1);
            int result = numberA + numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}