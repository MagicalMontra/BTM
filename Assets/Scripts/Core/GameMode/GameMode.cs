using System.Collections.Generic;
using HotPlay.QuickMath.Calculation;

namespace HotPlay.BoosterMath.Core.UI
{
    public class GameMode
    {
        public GameModeSettings Settings => settings;
        public List<IQuestionPattern> Complexity => complexity;

        private GameModeSettings settings;
        private List<IQuestionPattern> complexity;
        
        public GameMode(GameModeSettings settings, List<IQuestionPattern> complexity)
        {
            this.settings = settings;
            this.complexity = complexity;
        }
    }
}