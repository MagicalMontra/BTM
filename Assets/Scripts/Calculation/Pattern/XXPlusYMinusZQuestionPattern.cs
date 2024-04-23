using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXPlusYMinusZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 10)
                maxNumber = 20;
            
            int numberA = Random.Range(10, maxNumber + 1);
            int numberB = Random.Range(1, 10);
            int numberC = Random.Range(1, 10);
            var diff = Mathf.Abs(numberB - numberC);
            
            while (diff < 2 || numberB == numberC)
            {
                numberC = Random.Range(1, 10);
                diff = Mathf.Abs(numberB - numberC);
            }
            
            int result = numberA + numberB - numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Minus);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}