using System;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Gameplay;
using HotPlay.PecanUI.SceneLoader;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class Signals : MonoBehaviour
    {
        public void SendDailyLoginSignal()
        {
            Signal.Send("DailyLogin", "Open", "message");
        }

        public void SendTutorialSignal(TutorialData[] data)
        {
            Signal.Send("Gameplay", "Tutorial", data);
        }

        public void SendTutorialClosedSignal()
        {
            Signal.Send("Tutorial", "Close", "message");
        }

        public void SendPauseSignal()
        {
            Signal.Send("Gameplay", "Pause", "message");
        }

        public void SendResumeSignal()
        {
            Signal.Send("Gameplay", "ResumeRequest");
        }

        public void SendResultSignal(GameResultData resultData)
        {
            Signal.Send("Gameplay", "Result", resultData);
        }

        public void SendStartSignal()
        {
            Signal.Send("Gameplay", "StartRequest");
        }
    }
}
