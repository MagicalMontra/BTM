using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Gameplay
{
    public class GameplayCurrencyBar : CurrencyBar
    {
        private void Awake()
        {
            PecanServices.Instance.Events.GameplayCurrencyUpdate += OnCurrencyUpdate;
        }

        private void OnDestroy()
        {
            if (PecanServices.HasInstance)
            {
                PecanServices.Instance.Events.GameplayCurrencyUpdate -= OnCurrencyUpdate;
            }
        }
    }
}
