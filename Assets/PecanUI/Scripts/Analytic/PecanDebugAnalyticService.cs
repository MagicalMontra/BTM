using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Analytic
{
    public class PecanDebugAnalyticService : IAnalyticService
    {
        public void Log<TContract>(Func<TContract> eventAction)
        {
            Debug.Log(eventAction.Invoke().ToString());
        }

        public void Log<TContract>(IAnalyticEvent<TContract> @event)
        {
            Debug.Log(@event.GetEvent().ToString());
        }

        public void Log<TContract, TValue>(TValue value, IAnalyticEvent<TContract, TValue> @event)
        {
            Debug.Log(@event.GetEvent(value).ToString());
        }
    }
}