using System.Collections.Generic;
using System.Linq;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation
{
    public class XXDivideYDivideZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 20)
                maxNumber = 20;
            
            var primeFactors = new List<int>();
            int numberA = Random.Range(10, maxNumber + 1);
            var factors = primeFactors.Generate(numberA, 2, maxNumber + 1);
            while (factors.Count <= 1 || factors.Any(factor => factor > 9))
            {
                numberA = Random.Range(10, maxNumber + 1);
                factors = primeFactors.Generate(numberA, 2, 9);
            }
            int numberB = factors.RandomPick();
            factors.Remove(numberB);
            int numberC = factors.RandomPick();
            int result = numberA / numberB / numberC;
            var pairA = new NumberPair(numberA, OperatorEnum.Divide);
            var pairB = new NumberPair(numberB, OperatorEnum.Divide);
            var pairC = new NumberPair(numberC, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB, pairC);
        }
    }
}