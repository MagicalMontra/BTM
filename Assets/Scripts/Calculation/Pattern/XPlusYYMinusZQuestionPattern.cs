using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYYMinusZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 10)
                maxNumber = 20;
            
            int numberA = Random.Range(1, 10);
            int numberB = Random.Range(10, maxNumber + 1);
            int numberC = Random.Range(3, 10);
            int result = numberA + numberB - numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Minus);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}