using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMinusYZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int numberA = Random.Range(10, 16);
            int numberB = Random.Range(6, 10);
            int result = numberA - numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Minus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}