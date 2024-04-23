using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XMultiplyYZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 10)
                maxNumber = 50;
            
            int numberA = Random.Range(5, 10);
            int numberB = Random.Range(2, 10);
            
            while (numberA * numberB > maxNumber)
                numberB = Random.Range(2, 10);

            var swap = Random.Range(0, 1);
            
            int result = numberA * numberB;
            var pairA = swap == 0 ? new NumberPair(numberA, OperatorEnum.Multiply) : new NumberPair(numberB, OperatorEnum.Equal);
            var pairB = swap == 0 ? new NumberPair(numberB, OperatorEnum.Equal) : new NumberPair(numberA, OperatorEnum.Multiply);
            return new QuestionData(result, pairA, pairB);
        }
    }
}