using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMinusYYMinusZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 50)
                maxNumber = 50;
            
            int numberA = Random.Range(20, maxNumber + 1);
            var remain = numberA;
            int numberB = Random.Range(10, remain + 1);
            var diff = Mathf.Abs(numberA - numberB);
            while (diff < 5 || numberA == numberB)
            {
                numberB = Random.Range(10, remain + 1);
                diff = Mathf.Abs(numberA - numberB);
            }
            
            remain = Mathf.Clamp(numberA - numberB, 1, 10);
            int numberC = Random.Range(1, remain);
            int result = numberA - numberB - numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Minus);
            var pairB = new NumberPair(numberB, OperatorEnum.Minus);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}