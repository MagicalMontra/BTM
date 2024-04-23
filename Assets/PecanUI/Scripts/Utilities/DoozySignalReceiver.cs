using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace HotPlay.PecanUI
{
    /// <summary>
    /// The gameObject this class attach to must be active to be able to receive signal
    /// </summary>
    public class DoozySignalReceiver : MonoBehaviour
    {
        public event Action<Signal> SignalReceived;

        [SerializeField]
        private string signalCategory;

        [SerializeField]
        private string signalName;

        private SignalStream signalStream;
        private SignalReceiver signalReceiver;

        private void Awake()
        {
            signalStream = SignalStream.Get(signalCategory, signalName);
            signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
        }

        private void OnEnable()
        {
            signalStream.ConnectReceiver(signalReceiver);
        }

        private void OnDisable()
        {
            signalStream.DisconnectReceiver(signalReceiver);
        }

        private void OnSignal(Signal signal)
        {
            SignalReceived?.Invoke(signal);
        }
    }
}
