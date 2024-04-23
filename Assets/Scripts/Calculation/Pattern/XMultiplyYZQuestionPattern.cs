using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XMultiplyYZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int numberA = Random.Range(2, 10);
            int numberB = 10;
            
            while (numberA * numberB > 9)
            {
                numberB = Random.Range(1, 5);
            }

            int result = numberA * numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Multiply);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}