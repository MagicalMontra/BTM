using System.Collections.Generic;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation
{
    public class XDivideYZQuestionPattern : IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            int numberA = Random.Range(2, 10);
            var primeFactors = new List<int>();
            var factors = primeFactors.Generate(numberA, 2, numberA);
            
            while (factors.Count <= 0)
            {
                numberA = Random.Range(2, 10);
                factors = primeFactors.Generate(numberA, 2, 9);
            }
            
            int numberB = factors.RandomPick();
            int result = numberA / numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Divide);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}