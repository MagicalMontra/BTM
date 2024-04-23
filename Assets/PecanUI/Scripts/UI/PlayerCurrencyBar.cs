using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class PlayerCurrencyBar : CurrencyBar
    {
        private void Awake()
        {
            PecanServices.Instance.Events.SoftCurrencyUpdate += OnCurrencyUpdate;
        }

        private void OnDestroy()
        {
            if (PecanServices.HasInstance)
            {
                PecanServices.Instance.Events.SoftCurrencyUpdate -= OnCurrencyUpdate;
            }
        }
    }
}
