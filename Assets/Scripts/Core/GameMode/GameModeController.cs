using System;
using System.Collections.Generic;
using System.Linq;

namespace HotPlay.BoosterMath.Core.UI
{
    public class GameModeController
    {
        public Action<GameMode> OnGameModeChanged;
        public GameMode CurrentGameMode => currentGameMode;
        
        private GameMode currentGameMode;

        private readonly List<GameMode> modeList;

        public GameModeController(List<GameMode> modeList)
        {
            this.modeList = modeList;
        }

        public void ChangeMode(GameModeEnum modeEnum)
        {
            currentGameMode = modeList.First(mode => mode.Settings.GameModeEnum == modeEnum);
            OnGameModeChanged?.Invoke(currentGameMode);
        }
        
        public void DeselectMode()
        {
            currentGameMode = null;
        }
    }
}