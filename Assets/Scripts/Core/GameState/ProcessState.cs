using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using HotPlay.Utilities;
using MoreMountains.Feedbacks;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ProcessState : StateBase, IInitializable, IDisposable
    {
        public override GameStateEnum Name => GameStateEnum.Process;

        [Inject]
        private readonly SignalBus signalBus;

        [Inject]
        private readonly SoundData soundData;
                
        [Inject]
        private readonly HeartPanel heartPanel;
        
        [Inject]
        private readonly ITimer questionTimer;

        [Inject]
        private readonly PecanServices services;
        
        [Inject]
        private readonly MMF_Player cameraShaker;

        [Inject]
        private readonly SoundStatus soundStatus;

        [Inject]
        private readonly AnswerValidator validator;
        
        [Inject]
        private readonly GameModeController gameMode;

        [Inject]
        private readonly PlayerSpawner playerSpawner;
        
        [Inject]
        private readonly IEnemySpawner enemySpawner;

        [Inject]
        private readonly PauseController pauseController;

        [Inject]
        private readonly GameplayUIController gameplayUI;

        [Inject]
        private readonly QuestionController questionController;

        [Inject]
        private readonly ItemDropController itemDropController;

        [Inject]
        private readonly GameModeController gameModeController;

        [Inject]
        private readonly GameSessionController gameSessionController;

        [Inject]
        private readonly GlobalWorldVFXSettings globalWorldVFXSettings;

        [Inject]
        private readonly WorldSpaceVFXController worldSpaceVFXController;

        private int countDownSoundId = -1;
        
        private CancellationTokenSource cancellation;

        public void Initialize()
        {
            questionTimer.OnStop += OnTimerStopped;
            pauseController.OnPaused += OnPaused;
            questionTimer.OnTimedOut += OnTimeOut;
            questionTimer.OnTicked += OnTimerTicked;
            validator.onValidAnswered += OnValidAnswered;
            validator.onInvalidAnswered += OnInvalidAnswered;
            services.Events.GameplayEventsHandler.Restart += Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu += Cancel;
        }

        public void Dispose()
        {
            questionTimer.OnStop -= OnTimerStopped;
            pauseController.OnPaused -= OnPaused;
            questionTimer.OnTimedOut -= OnTimeOut;
            questionTimer.OnTicked -= OnTimerTicked;
            validator.onValidAnswered -= OnValidAnswered;
            validator.onInvalidAnswered -= OnInvalidAnswered;
            services.Events.GameplayEventsHandler.Restart -= Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu -= Cancel;
        }
        
        public override async UniTask Enter()
        {
            cancellation?.Dispose();
            cancellation = new CancellationTokenSource();
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            worldSpaceVFXController.Dispose();
            pauseController.SetPauseStatus(true);
            itemDropController.PreDrop();
            playerSpawner.Current.Idle(cancellation.Token);
            await UniTask.Yield();
        }

        public override async UniTask Execute()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            var selectedComplexity = gameMode.CurrentGameMode.Complexity.RandomPick();
            gameplayUI.ShowAsync(selectedComplexity.GetQuestion(gameMode.CurrentGameMode.Settings.MaxAnswerRange)).Forget();
            await questionController.Panel.QuestionTimeGauge.SetItemDropActive(true, cancellation.Token);

            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                return;
            }
            
            await UniTask.Yield();
        }

        public override async UniTask Exit()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            questionController.Panel.QuestionTimeGauge.Show();
            questionTimer.Start().Forget();
            questionTimer.Pause(false);
            await UniTask.Yield();
        }

        private void Cancel()
        {
            cancellation?.Dispose();
            cancellation = new CancellationTokenSource();
            cancellation?.Cancel();
        }

        private void OnTimerStopped()
        {
            gameplayUI.Hide();
            if (services.SoundManager.SoundPlayer.IsPlaying(countDownSoundId))
                services.SoundManager.SoundPlayer.Stop(countDownSoundId);

            questionController.Panel.QuestionTimeGauge.HideAsync(cancellation.Token).Forget();
        }

        private void OnTimeOut()
        {
            OnTimeOutAsync().Forget();
        }

        private async UniTaskVoid OnTimeOutAsync()
        {
            pauseController.SetPauseStatus(false);
            
            if (cancellation.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            services.SoundManager.SoundPlayer.Play(soundData.TimeOut, false);
            await gameplayUI.HideAsync().AttachExternalCancellation(cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            Assert.IsNotNull(enemySpawner.Current);
            var thunderVfx = worldSpaceVFXController.Spawn(globalWorldVFXSettings.playerTimeOutDeathVfx, playerSpawner.Current.transform.position);
            cameraShaker.PlayFeedbacks();
            services.SoundManager.SoundPlayer.Play(soundData.PlayerDefeated, false);
            playerSpawner.Despawn();
            await thunderVfx.Activate(cancellation.Token);

            if (cancellation.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            heartPanel.UpdateHeart(0);
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Termination));
        }

        private void OnTimerTicked()
        {
            if (questionTimer.Counter > questionTimer.Duration * 0.3f)
                return;
            
            if (services.SoundManager.SoundPlayer.IsPlaying(countDownSoundId))
                return;

            countDownSoundId = services.SoundManager.SoundPlayer.Play(soundData.TimerCountDown, true);
        }

        private void OnPaused(bool isPaused)
        {
            if (isPaused)
            {
                if (services.SoundManager.SoundPlayer.IsPlaying(countDownSoundId))
                    services.SoundManager.SoundPlayer.Stop(countDownSoundId);
                
                if (soundStatus.bgmStatus)
                    services.SoundManager.SoundPlayer.SetEnableBGM(false);
                
                return;
            }
            
            if (soundStatus.bgmStatus)
                services.SoundManager.SoundPlayer.SetEnableBGM(true);
            
            if (questionTimer.Counter > questionTimer.Duration * 0.3f)
                return;

            countDownSoundId = services.SoundManager.SoundPlayer.Play(soundData.TimerCountDown, true);
        }
        
        private void OnValidAnswered()
        {
            TryOnValidAnswered().Forget();
        }

        private void OnInvalidAnswered()
        {
            TryOnInvalidAnswered().Forget();
        }

        private async UniTaskVoid TryOnValidAnswered()
        {
            services.SoundManager.SoundPlayer.Play(soundData.CorrectAnswer, false);
            var timeLeft = questionTimer.Counter;
            var timeThreshold = questionTimer.Duration * gameModeController.CurrentGameMode.Settings.ItemDropCurve.Evaluate(gameSessionController.CurrentStage / gameSessionController.CurrentStage + 10);

            if (timeLeft >= timeThreshold)
            {
                itemDropController.MarkDrop();
                questionController.Panel.QuestionTimeGauge.SetItemDropActive(false);
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                questionTimer.Stop();
                await UniTask.Yield();
                return;
            }
            
            questionTimer.Pause(true);
            if (itemDropController.IsBoosterActivate(ItemDropTypeEnum.Rewind))
                await questionTimer.Reset(cancellation.Token);
            else
                await questionTimer.AddTime(cancellation.Token);
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);

            questionTimer.Stop();

            if (cancellation is { IsCancellationRequested: true })
            {
                await UniTask.Yield();
                return;
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            await UniTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellation.Token);
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation is { IsCancellationRequested: true })
            {
                await UniTask.Yield();
                return;
            }
            
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.AnimatePlayer));
        }
        
        private async UniTaskVoid TryOnInvalidAnswered()
        {
            questionController.Panel.QuestionTimeGauge.SetItemDropActive(false);
            services.SoundManager.SoundPlayer.Play(soundData.WrongAnswer, false);
            await UniTask.Delay(500, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellation.Token);
            questionTimer.Stop();

            if (cancellation is { IsCancellationRequested: true })
            {
                await UniTask.Yield();
                return;
            }
            
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.AnimateEnemy));
        }
    }
}