using System;
using UnityEngine;
using Doozy.Runtime.Signals;

namespace HotPlay.PecanUI.Events
{
    public class TutorialEventsHandler : MonoBehaviour
    {
        public event Action CloseButtonClicked;
        public event Action NextButtonClicked;
        public event Action PreviousButtonClicked;
        
        private SignalStream tutorialSignalStream;
        private SignalReceiver tutorialSignalReceiver;

        [SerializeField]
        private TutorialDialog tutorialDialog;

        private void Start()
        {
            tutorialSignalStream = SignalStream.Get("Gameplay", "Tutorial");
            tutorialSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
            tutorialSignalStream.ConnectReceiver(tutorialSignalReceiver);
        }

        public void InvokeCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
        }

        public void InvokeNextButtonClicked()
        {
            NextButtonClicked?.Invoke();
        }

        public void InvokePreviousButtonClicked()
        {
            PreviousButtonClicked?.Invoke();
        }

        private void OnSignal(Signal signal)
        {
            var tutorialData = signal.GetValueUnsafe<TutorialData[]>();
            tutorialDialog.SetupData(tutorialData);
        }
    }
}
