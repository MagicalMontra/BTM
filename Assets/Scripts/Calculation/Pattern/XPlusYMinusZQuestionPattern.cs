using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYMinusZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int numberA = Random.Range(1, 10);
            int numberB = Random.Range(1, 10);
            
            var diff = Mathf.Abs(numberA - numberB);

            while (diff < 2)
            {
                numberB = Random.Range(1, 10);
                diff = Mathf.Abs(numberA - numberB);
            }
            
            int remain = numberA + numberB > 10 ? 10 : numberA + numberB;
            int numberC = Random.Range(1, remain);
            diff = Mathf.Abs(numberB - numberC);

            while (diff < 2)
            {
                numberC = Random.Range(1, remain);
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