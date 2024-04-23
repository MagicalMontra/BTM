using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.DailyLogin;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class DailyLoginEventsHandler : MonoBehaviour
    {
        public event Action<DailyRewardDayContainer> ElementClicked;
        public event Action CloseButtonClicked;

        public void InvokeElementClicked(DailyRewardDayContainer reward)
        {
            ElementClicked?.Invoke(reward);
        }

        public void InvokeCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
        }
    }
}
