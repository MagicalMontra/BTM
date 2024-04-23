using System;

namespace HotPlay.QuickMath.Calculation
{
    public class QuestionData
    {
        public int Result => result;

        public NumberPair[] Pairs => pairs;

        private int result;
        
        private NumberPair[] pairs;
        
        public QuestionData(int result, params NumberPair[] pairs)
        {
            this.result = result;
            this.pairs = pairs;
        }
    }
}
