using System;
using System.Collections.Generic;

namespace HotPlay.QuickMath.Calculation
{
    public static class PrimeNumberWorker
    {
        public static List<int> Generate(this List<int> primeFactors, int number, int min, int max)
        {
            primeFactors.Clear();
            
            for (int div = min; div <= number; div++)
            {
                if (div == number)
                    continue;
                
                if (div > max)
                    break;
                
                if (number % div == 0)
                    primeFactors.Add(div);
                
            }
    
            return primeFactors;
        }
    }
}