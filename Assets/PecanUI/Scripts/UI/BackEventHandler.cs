#nullable enable

using Doozy.Runtime.Nody;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class BackEventHandler : MonoBehaviour
    {
        [SerializeField]
        private FlowController flowController = default!;

        private SignalReceiver backSignal = null!;
        private Dictionary<string, Action> callbacks = new Dictionary<string, Action>();

        private void Awake()
        {
            backSignal = new SignalReceiver().SetOnSignalCallback(OnBackButtonNotified);
        }

        private void OnValidate()
        {
            if (flowController == null)
            {
                throw new Exception($"{nameof(flowController)} is null");
            }
        }

        private void OnEnable()
        {
            BackButton.stream.ConnectReceiver(backSignal);
        }

        private void OnDisable()
        {
            BackButton.stream.DisconnectReceiver(backSignal);
        }

        /// <summary>
        /// Listen to all Back event
        /// </summary>
        /// <param name="callback">Callback</param>
        public void SubscribeAll(Action callback)
        {
            Subscribe("", callback);
        }

        /// <summary>
        /// Listen to Back event occurred while this nodeName is active
        /// </summary>
        /// <param name="nodeName">Name of active node</param>
        /// <param name="callback">Callback</param>
        public void Subscribe(string nodeName, Action callback)
        {
            if (!callbacks.ContainsKey(nodeName))
            {
                callbacks.Add(nodeName, null!);
            }
            callbacks[nodeName] += callback;
        }

        public void UnsubscribeAll(Action callback)
        {
            Unsubscribe("", callback);
        }

        public void Unsubscribe(string nodeName, Action callback)
        {
            if (callbacks.ContainsKey(nodeName))
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                callbacks[nodeName] -= callback;
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }

        private void OnBackButtonNotified(Signal signal)
        {
            var currentNodeName = flowController.flow.activeNode.nodeName;
            Invoke(callbacks, currentNodeName);
            Invoke(callbacks, "");

            static void Invoke(Dictionary<string, Action> callbacks, string key)
            {
                if (callbacks.TryGetValue(key, out var callback))
                {
                    callback.Invoke();
                }
            }
        }
    }
}
