using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYYMinusZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 10)
                maxNumber = 10;
            
            int numberA = Random.Range(1, 10);
            int numberB = Random.Range(10, maxNumber + 1);
            int numberC = Random.Range(10, maxNumber + 1);
            var diff = Mathf.Abs(numberB - numberC);

            while (numberB == numberC || diff < 5)
            {
                numberC = Random.Range(10, maxNumber + 1);
                diff = Mathf.Abs(numberB - numberC);
            }

            if (numberB < numberC)
                (numberB, numberC) = (numberC, numberB);

            int result = numberA + numberB - numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Minus);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}