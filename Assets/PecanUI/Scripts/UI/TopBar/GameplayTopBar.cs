using HotPlay.PecanUI.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class GameplayTopBar : BaseTopBar
    {
        [SerializeField]
        private GameplayCurrencyBar gameplayCurrencyBar;
        public GameplayCurrencyBar GameplayCurrencyBar => gameplayCurrencyBar;
    }
}
