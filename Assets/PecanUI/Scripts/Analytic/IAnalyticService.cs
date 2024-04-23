using System;
using System.Collections.Generic;

namespace HotPlay.PecanUI.Analytic
{
    public interface IAnalyticService
    {
        void Log<TContract>(Func<TContract> eventAction);
        void Log<TContract>(IAnalyticEvent<TContract> @event);
        void Log<TContract, TValue>(TValue value, IAnalyticEvent<TContract, TValue> @event);
    }
}