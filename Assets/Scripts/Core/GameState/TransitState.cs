using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class TransitState : StateBase, IInitializable, IDisposable
    {
        public override GameStateEnum Name => GameStateEnum.Transit;
        
        [Inject]
        private SignalBus signalBus;
        
        [Inject]
        private SoundData soundData;
        
        [Inject]
        private PecanServices services;
        
        [Inject]
        private IEnemySpawner enemySpawner;
        
        [Inject]
        private PlayerSpawner playerSpawner;

        [Inject]
        private GameplayUIController gameplayUI;
        
        [Inject]
        private PauseController pauseController;

        [Inject]
        private ItemDropController itemDropController;
        
        [Inject]
        private TutorialController tutorialController;
        
        [Inject]
        private ParallaxController parallaxController;
        
        [Inject]
        private GameModeController gameModeController;
        
        [Inject]
        private WorldSpaceVFXController worldSpaceVFXController;

        private bool isMoving;
        private int soundId = -1;
        private CancellationTokenSource cancellation;
        
        private List<UniTask> tasks = new List<UniTask>();

        public void Initialize()
        {
            pauseController.OnPaused += OnPaused;
            services.Events.GameplayEventsHandler.Restart += Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu += Cancel;
        }

        public void Dispose()
        {
            pauseController.OnPaused -= OnPaused;
            services.Events.GameplayEventsHandler.Restart -= Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu -= Cancel;
        }
        
        private void Cancel()
        {
            cancellation?.Dispose();
            cancellation = new CancellationTokenSource();
            cancellation?.Cancel();
            
            if (services.SoundManager.SoundPlayer.IsPlaying(soundId))
                services.SoundManager.SoundPlayer.Stop(soundId);
        }
        
        public override async UniTask Enter()
        {
            cancellation?.Dispose();
            cancellation = new CancellationTokenSource();
            
            tasks.Clear();
            await gameplayUI.HideAsync();
        }

        public override async UniTask Execute()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
        
            isMoving = true;

            if (playerSpawner.Current != null)
            {
                if (!playerSpawner.Current.IsFloating)
                    soundId = services.SoundManager.SoundPlayer.Play(soundData.PlayerStep, true);
            
                playerSpawner.Current.Walk(cancellation.Token);
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            tasks.Clear();
            tasks.Add(itemDropController.Collect());
            tasks.Add(enemySpawner.Despawn());
            tasks.Add(parallaxController.Current.MoveForSeconds(1.5f, cancellation.Token));
            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current?.Idle(cancellation.Token);
                services.SoundManager.SoundPlayer.Stop(soundId);
                parallaxController.Current.Stop();
                isMoving = false;
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            tutorialController.EngageBoosterTutorialQueue().Forget();
            services.SoundManager.SoundPlayer.Stop(soundId);
            playerSpawner.Current?.Idle(cancellation.Token);
            isMoving = false;
        }

        public override async UniTask Exit()
        {
            worldSpaceVFXController.Dispose();
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            services.SoundManager.SoundPlayer.Play(soundData.MonsterSpawn, false);
            
            if (gameModeController.CurrentGameMode != null)
                await enemySpawner.Spawn(cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Process));
        }

        private void OnPaused(bool isPaused)
        {
            if (isPaused)
            {
                if (services.SoundManager.SoundPlayer.IsPlaying(soundId))
                    services.SoundManager.SoundPlayer.Stop(soundId);
                
                return;
            }
            
            if (playerSpawner.Current == null)
                return;
            
            if (isMoving && !playerSpawner.Current.IsFloating)
                soundId = services.SoundManager.SoundPlayer.Play(soundData.PlayerStep, true);
        }
    }
}