using System;

namespace HotPlay.QuickMath.Calculation
{
    public class NumberPair
    {
        public int Number => number;

        public OperatorEnum Symbol => symbol;

        private int number;
        private OperatorEnum symbol;
        
        public NumberPair(int number, OperatorEnum symbol)
        {
            this.number = number;
            this.symbol = symbol;
        }
    }

    [Serializable]
    public enum OperatorEnum
    {
        Plus = '+',
        Minus = '-',
        Multiply = 'x',
        Divide = '÷',
        Equal = '='
    }
}