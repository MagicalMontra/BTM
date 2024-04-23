using System.Collections.Generic;
using System.Linq;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation
{
    public class XXDivideYYZQuestionPattern :IQuestionPattern
    {
        public QuestionData GetQuestion(int maxNumber)
        {
            if (maxNumber < 20)
                maxNumber = 20;
            
            var primeFactors = new List<int>();
            int numberA = Random.Range(10, maxNumber + 1);
            var factors = primeFactors.Generate(numberA, 10, numberA - 1);
            
            while (factors.Count <= 0 || factors.All(factor => numberA / factor > 9))
            {
                numberA = Random.Range(10, maxNumber + 1);
                factors = primeFactors.Generate(numberA, 10, numberA - 1);
            }
            
            int numberB = factors.Where(factor => numberA / factor < 10).ToArray().RandomPick();
            int result = numberA / numberB;
            var pairA = new NumberPair(numberA, OperatorEnum.Divide);
            var pairB = new NumberPair(numberB, OperatorEnum.Equal);
            return new QuestionData(result, pairA, pairB);
        }
    }
}