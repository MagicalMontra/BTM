using System;
using Doozy.Runtime.Signals;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using HotPlay.goPlay.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotPlay.PecanUI.SceneLoader
{
    public class PecanSceneHandler : MonoBehaviour
    {
        [SerializeField]
        private float fakeLoadTime = 1.5f;
        
        [SerializeField]
        private float fakeLoadTimeThreshold = 0.5f;
        
        [SerializeField]
        private LoadingDialog loadingDialog;

        private Action callback;
        private ISceneLoader sceneLoader;
        private SignalReceiver signalReceiver; // Signal receiver
        private SignalStream signalStream;     // Target stream

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            signalStream = SignalStream.Get("SceneTransition", "Load");
            signalReceiver = new SignalReceiver().SetOnSignalCallback(OnLoadSignal);
            signalStream.ConnectReceiver(signalReceiver);
        }

        public void Initialize(ISceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        private void Initialize()
        {
            sceneLoader = new PecanSceneLoader(fakeLoadTime, fakeLoadTimeThreshold, loadingDialog.Progressor);
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoadAsync(scene).Forget();
        }

        private async UniTaskVoid OnSceneLoadAsync(Scene scene)
        {
            callback?.Invoke();
            await UniTask.WaitUntil(() => loadingDialog.isClosed);
            Signal.Send("SceneTransition", $"Load{scene.name}Complete");
            callback = null;
        }

        private void OnLoadSignal(Signal signal)
        {
            if (!signal.TryGetValue<SceneLoadSignalData>(out var signalData)) 
                return;
            
            callback = signalData.Callback;
            WaitForScene(signalData.Scene).Forget();
        }

        private async UniTask WaitForScene(SceneReference scene)
        {
            if (sceneLoader == null)
            {
                Initialize();
                Debug.LogWarning("Custom scene loader is not found, initializing pecan scene loader.");
            }

            Signal.Send("MenuUI", "Loading");
            await UniTask.WaitUntil(() => loadingDialog.isReadied);
            await sceneLoader.LoadUniTask(scene);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            signalStream.DisconnectReceiver(signalReceiver);
        }
    }
}
