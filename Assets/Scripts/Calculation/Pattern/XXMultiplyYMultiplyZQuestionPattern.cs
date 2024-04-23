using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMultiplyYMultiplyZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 20)
                maxNumber = 20;
            
            int numberA = Random.Range(10, maxNumber + 1);
            int maxProduct = 1000 / numberA;
            int numberB = Random.Range(1, Mathf.Min(maxProduct, 9));
            maxProduct = 1000 / (numberA * numberB);
            int numberC = Random.Range(1, Mathf.Min(maxProduct, 9));
            int result = numberA * numberB * numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Multiply);
            var pairB = new NumberPair(numberB, OperatorEnum.Multiply);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}