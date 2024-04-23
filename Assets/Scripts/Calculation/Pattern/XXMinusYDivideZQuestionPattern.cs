using System.Collections.Generic;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation
{
    public class XXMinusYDivideZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 20)
                maxNumber = 20;
            
            var primeFactors = new List<int>();
            int numberA = Random.Range(10, maxNumber + 1);
            int numberB = Random.Range(1, 10);
            var factors = primeFactors.Generate(numberB, 2, 9);
            while (factors.Count <= 0)
            {
                numberB = Random.Range(1, 10);
                factors = primeFactors.Generate(numberB, 2, 9);
            }
            
            int numberC = factors.RandomPick();
            int result = numberA - numberB / numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Minus);
            var pairB = new NumberPair(numberB, OperatorEnum.Divide);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}