using System;
using DG.Tweening;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.Gameplay;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class PecanInitializerTest : MonoSingleton<PecanInitializerTest>
    {
        [SerializeField]
        private Renderer renderer;
        
        [SerializeField]
        private Transform playModeTestObject;

        private PecanDebugAnalyticService service;

        private bool isPlaying;
        
        private Sequence sequence;
        private SignalStream resultStream;
        private SignalReceiver resultReceiver;
        
        private void Start()
        {
            service = new PecanDebugAnalyticService();
            resultStream = SignalStream.Get("Gameplay", "Result");
            resultReceiver = new SignalReceiver().SetOnSignalCallback(OnGameResult);
            resultStream.ConnectReceiver(resultReceiver);
            
            if (!PecanServices.HasInstance)
                return;

            PecanServices.Instance.Analytic.Initialize(service);
            PecanServices.Instance.Events.PauseEventsHandler.Pause += PlayModePause;
            PecanServices.Instance.Events.PauseEventsHandler.Resume += OnGameplayResumed;
            PecanServices.Instance.Events.GameplayEventsHandler.Play += OnGameplayStarted;
            PecanServices.Instance.Events.GameplayEventsHandler.Restart += OnGameplayRestart;
            PecanServices.Instance.Events.GameplayEventsHandler.BackToMainMenu += OnMainMenuEntered;
        }

        private void Update()
        {
            if (!isPlaying)
                return;
            
            renderer.material.color = new Color(Mathf.Sin(Time.time), Mathf.Cos(Time.time), Mathf.Tan(Time.time), 1);
        }

        public override void OnDestroy()
        {
            resultStream.DisconnectReceiver(resultReceiver);
            
            if (!PecanServices.HasInstance)
                return;

            PecanServices.Instance.Events.PauseEventsHandler.Pause -= PlayModePause;
            PecanServices.Instance.Events.PauseEventsHandler.Resume -= OnGameplayResumed;
            PecanServices.Instance.Events.GameplayEventsHandler.Play -= OnGameplayStarted;
            PecanServices.Instance.Events.GameplayEventsHandler.Restart -= OnGameplayRestart;
            PecanServices.Instance.Events.GameplayEventsHandler.BackToMainMenu -= OnMainMenuEntered;
        }
        private  void OnGameResult(Signal signal)
        {
            PlayModeStop();
        }

        private void PlayModeStart()
        {
            isPlaying = true;
            sequence?.Kill();
            playModeTestObject.localScale = Vector3.one;
            playModeTestObject.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(playModeTestObject.DOScale(2, 2f));
            sequence.Append(playModeTestObject.DOScale(1, 2f));
            sequence.SetLoops(-1);
            sequence.Play();
        }
        
        private void PlayModePause()
        {
            sequence?.Pause();
            isPlaying = false;
        }

        private void PlayModeStop()
        {
            isPlaying = false;
            sequence?.Kill();
            playModeTestObject.gameObject.SetActive(false);
        }
        
        private void OnGameplayStarted()
        {
            PlayModeStart();
        }
        
        private void OnGameplayRestart()
        {
            PlayModeStart();
        }
        
        private void OnGameplayResumed()
        {
            isPlaying = true;
            sequence?.Play();
        }
        
        private void OnMainMenuEntered()
        {
            PlayModeStop();
        }
    }
}