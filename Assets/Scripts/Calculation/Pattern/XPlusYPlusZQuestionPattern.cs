using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HotPlay.QuickMath.Calculation
{
    public class XPlusYPlusZQuestionPattern : IQuestionPattern
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
            
            int numberC = Random.Range(1, 10);
            
            diff = Mathf.Abs(numberB - numberC);
            
            while (diff < 2)
            {
                numberC = Random.Range(1, 10);
                diff = Mathf.Abs(numberB - numberC);
            }
            
            int result = numberA + numberB + numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Plus);
            var pairB = new NumberPair(numberB, OperatorEnum.Plus);
            var pairC = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}