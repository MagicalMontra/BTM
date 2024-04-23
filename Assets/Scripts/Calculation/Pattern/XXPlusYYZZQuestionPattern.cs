using UnityEngine;

namespace HotPlay.QuickMath.Calculation
{
    public class XXPlusYYZZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 20)
                maxNumber = 20;
            
            int numberA = Random.Range(10, maxNumber + 1);
            int numberB = Random.Range(10, maxNumber + 1);
            var diff = Mathf.Abs(numberA - numberB);
            
            while (diff < 5)
            {
                numberB = Random.Range(10, maxNumber);
                diff = Mathf.Abs(numberA - numberB);
            }
            
            int result = numberA + numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}