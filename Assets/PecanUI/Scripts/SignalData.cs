using System;
using Doozy.Runtime.Signals;
using Eflatun.SceneReference;
using HotPlay.PecanUI.SceneLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPlay.PecanUI
{
    [Serializable]
    public class SignalData
    {
        [SerializeField]
        private string category;
        
        [SerializeField] 
        private bool canBeLoaded;
        
        [SerializeField]
        private string streamRequestName;
        
        [SerializeField]
        private string streamResponseName;

        [HideIf("NeedToShowScene")]
        [SerializeField]
        private SceneReference sceneToLoad;
        
#if UNITY_EDITOR
        private bool NeedToShowScene()
        {
            return !canBeLoaded;
        }
#endif
        
        private SignalStream requestStream;
        private SignalReceiver requestReceiver;
        private Func<string, bool> onRequest;

        public void  Initialize(Func<string, bool> onRequest)
        {
            this.onRequest += onRequest;
            requestStream = SignalStream.Get(category, streamRequestName);
            requestReceiver = new SignalReceiver().SetOnSignalCallback(OnRequest);
            requestStream.ConnectReceiver(requestReceiver);
        }

        public void Dispose()
        {
            onRequest = null;
            requestStream.DisconnectReceiver(requestReceiver);
        }

        private void OnRequest(Signal signal)
        {
            if (onRequest == null)
                return;
            
            var allowTransition = onRequest?.Invoke(category) ?? false;

            if (!allowTransition)
                return;

            if (PecanServices.HasInstance && PecanServices.Instance.NeedSceneChanged && canBeLoaded)
            {
                var signalData = new SceneLoadSignalData(sceneToLoad, () => TryTransit(signal));
                Signal.Send("SceneTransition", "Load", signalData);
                return;
            }

            TryTransit(signal);
        }

        private void TryTransit(Signal signal)
        {
            Signal.Send(category, streamResponseName, signal.message);
        }
    }
}