using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PauseController : MonoBehaviour
    {
        public Action<bool> OnPaused;
        
        public bool IsPause => isPause;
        
        private bool isPause;
        private bool isAbleToPause;
        private bool canPause;
        private bool isOnTutorialPause;
        private bool isApplicationFocused;

        private CancellationTokenSource cancelFocus;
        
        [Inject]
        private PecanServices services;

        [Inject]
        private GameStateProcessor gameStateProcessor;
        
        [Inject]
        private RewirdInputController inputController;

        private void Start()
        {
            services.Events.PauseEventsHandler.Pause += Pause;
            services.Events.PauseEventsHandler.Resume += Resume;
            services.Events.GameplayEventsHandler.Restart += Resume;
            gameStateProcessor.OnGameStateChanged += OnStateChanged;
            services.Events.GameplayEventsHandler.BackToMainMenu += Resume;
            services.Events.TutorialEventsHandler.CloseButtonClicked += OnTutorialUnpaused;
            services.Events.GameplayEventsHandler.TutorialButtonClicked += PauseTutorial;
        }

        private void OnDestroy()
        {
            services.Events.PauseEventsHandler.Pause -= Pause;
            services.Events.PauseEventsHandler.Resume -= Resume;
            services.Events.GameplayEventsHandler.Restart -= Resume;
            gameStateProcessor.OnGameStateChanged -= OnStateChanged;
            services.Events.GameplayEventsHandler.BackToMainMenu -= Resume;
            services.Events.TutorialEventsHandler.CloseButtonClicked -= OnTutorialUnpaused;
            services.Events.GameplayEventsHandler.TutorialButtonClicked -= PauseTutorial;
        }

        private void OnStateChanged(GameStateEnum state)
        {
            var isOnMainMenu = state is GameStateEnum.MainMenu;
            var isGameTerminating = state is GameStateEnum.Termination;
            var isGameReinitializing = state is GameStateEnum.Reinitialization;
            isAbleToPause = !isGameTerminating && !isOnMainMenu && !isGameReinitializing;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!services.GetCustomGamePlayPanel<GameplayUI>().gameObject.activeInHierarchy)
                return;
            
            if (hasFocus && !isApplicationFocused)
            {
                TryUnfocusResume();
                return;
            }
            
            isApplicationFocused = hasFocus;
            
            if (hasFocus)
                return;
            
            if (!isAbleToPause)
                return;

            cancelFocus?.Dispose();
            cancelFocus = new CancellationTokenSource();
            TryUnfocusPause(cancelFocus.Token).Forget();
        }

        public void Pause()
        {
            if (!isAbleToPause)
                return;
            
            TryPause().Forget();
        }

        public void PauseToggle()
        {
            if (!isAbleToPause)
                return;

            if (isOnTutorialPause)
            {
                OnTutorialUnpaused();
                return;
            }

            if (isPause)
            {
                services.Signals.SendResumeSignal();
                return;
            }
            
            services.Signals.SendPauseSignal();
        }

        public void PauseTutorial()
        {
            Pause();
            OnTutorialPaused();
        }

        public void SetPauseStatus(bool canPause)
        {
            this.canPause = canPause;
            services.GetDialog<GameplayDialog>().PauseButton.interactable = this.canPause;
            services.GetDialog<GameplayDialog>().TutorialButton.interactable = this.canPause;

            if (canPause)
            {
                inputController.BackKeyDown -= PauseToggle;
                inputController.BackKeyDown += PauseToggle;
                return;
            }

            inputController.BackKeyDown -= PauseToggle;
        }

        private async UniTaskVoid TryUnfocusPause(CancellationToken cancellationToken)
        {
            await UniTask.WaitWhile(() => gameStateProcessor.LastState == GameStateEnum.Transit, PlayerLoopTiming.Update, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            await UniTask.WaitWhile(() => gameStateProcessor.LastState == GameStateEnum.AnimateEnemy, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            await UniTask.WaitWhile(() => gameStateProcessor.LastState == GameStateEnum.AnimatePlayer, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            await UniTask.WaitWhile(() => gameStateProcessor.LastState == GameStateEnum.Initialization, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            await UniTask.WaitWhile(() => gameStateProcessor.LastState == GameStateEnum.Reinitialization, PlayerLoopTiming.Update, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            if (!isAbleToPause)
                return;
            
            services.Signals.SendPauseSignal();
        }

        private void TryUnfocusResume()
        {
            cancelFocus?.Cancel();
        }

        private async UniTaskVoid TryPause()
        {
            if (isPause)
            {
                await UniTask.Yield();
                return;
            }

            isPause = true;
            OnPaused?.Invoke(isPause);
        }

        private void OnTutorialPaused()
        {
            isOnTutorialPause = true;
        }
        
        private void OnTutorialUnpaused()
        {
            isOnTutorialPause = false;
            Resume();
        }

        private void Resume()
        {
            var isGameTerminating = gameStateProcessor.LastState is GameStateEnum.Termination;

            if (isGameTerminating)
                return;
            
            if (!isPause)
                return;
            
            isPause = false;
            OnPaused?.Invoke(isPause);
        }
    }
}