using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class PauseEventsHandler : MonoBehaviour
    {
        public event Action Pause;
        public event Action Resume;
        
        [Obsolete("deprecated, in case of binding return to main menu game logic, please use GameplayEventsHandler.BackToMainMenu.", true)]
        public event Action HomeButtonClicked;
        
        [Obsolete("deprecated, in case of binding return to main menu game logic, please use GameplayEventsHandler.Restart.", true)]
        public event Action RestartButtonClicked;
        public event Action TutorialButtonClicked;
        
        [Obsolete("deprecated, in case of binding resume game logic, please use PauseEventHandler.Resume.", true)]
        public event Action ResumeButtonClicked;
        
        private SignalStream pauseSignalStream;
        private SignalReceiver pauseSignalReceiver;

        private SignalStream resumeSignalStream;
        private SignalReceiver resumeSignalReceiver;

        private void Start()
        {
            pauseSignalStream = SignalStream.Get("Gameplay", "Pause");
            pauseSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnPauseSignal);
            pauseSignalStream.ConnectReceiver(pauseSignalReceiver);

            resumeSignalStream = SignalStream.Get("Gameplay", "ResumeResponse");
            resumeSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnResumeSignal);
            resumeSignalStream.ConnectReceiver(resumeSignalReceiver);
        }
        private void OnPauseSignal(Signal signal)
        {
            Pause?.Invoke();
        }

        private void OnResumeSignal(Signal signal)
        {
            Resume?.Invoke();
        }

        public void InvokeTutorialButtonClicked()
        {
            TutorialButtonClicked?.Invoke();
        }
    }
}
