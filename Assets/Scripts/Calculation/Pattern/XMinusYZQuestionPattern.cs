using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XMinusYZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            var remain = 9;
            int numberA = Random.Range(5, 10);
            remain -= numberA;
            int numberB = Random.Range(2, remain + 1);

            if (numberA < numberB)
                (numberA, numberB) = (numberB, numberA);
            
            int result = numberA - numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Minus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}