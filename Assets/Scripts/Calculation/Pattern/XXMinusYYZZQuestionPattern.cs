using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMinusYYZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 30)
                maxNumber = 30;
            
            int numberA = Random.Range(20, maxNumber + 1);
            int numberB = Random.Range(10, maxNumber + 1);
            var diff = Mathf.Abs(numberA - numberB);
            
            while (diff < 10 || numberA == numberB)
            {
                numberB = Random.Range(10, maxNumber + 1);
                diff = Mathf.Abs(numberA - numberB);
            }

            if (numberB > numberA)
                (numberA, numberB) = (numberB, numberA);
            
            int result = numberA - numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Minus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}