using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class CommonTopBar : BaseTopBar
    {
        [SerializeField]
        private PlayerCurrencyBar playerCurrencyBar;
        public PlayerCurrencyBar PlayerCurrencyBar => playerCurrencyBar;
    }
}
