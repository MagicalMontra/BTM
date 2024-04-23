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
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PlayerState : StateBase, IInitializable, IDisposable
    {
        public override GameStateEnum Name => GameStateEnum.AnimatePlayer;

        [Inject]
        private SignalBus signalBus;
        
        [Inject]
        private SoundData soundData;

        [Inject]
        private PecanServices services;
        
        [Inject]
        private MMF_Player cameraShaker;

        [Inject]
        private IEnemySpawner enemySpawner;
        
        [Inject]
        private PlayerSpawner playerSpawner;
        
        [Inject]
        private PauseController pauseController;

        [Inject]
        private ItemDropController itemDropController;
        
        [Inject]
        private GameModeController gameModeController;
        
        [Inject]
        private GameSessionController gameSessionController;

        [Inject]
        private WorldSpaceVFXController worldSpaceVFXController;

        private Vector3 playerPos;

        private ICharacter currentEnemy;
        
        private CancellationTokenSource cancellation;
        
        public void Initialize()
        {
            services.Events.GameplayEventsHandler.Restart += Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu += Cancel;
        }

        public void Dispose()
        {
            services.Events.GameplayEventsHandler.Restart -= Cancel;
            services.Events.GameplayEventsHandler.BackToMainMenu -= Cancel;
        }
        
        private void Cancel()
        {
            cancellation?.Dispose();
            cancellation = new CancellationTokenSource();
            cancellation?.Cancel();
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
            
            services.SoundManager.SoundPlayer.Play(soundData.PlayerAttack, false);
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
            
            currentEnemy = enemySpawner.Current;
            if (playerSpawner.Current.AttackVfx != null)
            {
                var attackVfx = worldSpaceVFXController.Spawn(playerSpawner.Current.AttackVfx,  playerSpawner.Current.transform.position);
                attackVfx.Activate(cancellation.Token).Forget();
            }

            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current.Idle(cancellation.Token);
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            await playerSpawner.Current.Attack(cancellation.Token);
 
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current.Idle(cancellation.Token);
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current.Idle(cancellation.Token);
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            cameraShaker.PlayFeedbacks();
            itemDropController.Drop();
            playerSpawner.Current.Idle(cancellation.Token);
            currentEnemy = enemySpawner.Current;
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current.Idle(cancellation.Token);
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            var task = currentEnemy?.GetHit(playerSpawner.Current.AttackPower, cancellation.Token);
            Assert.IsTrue(task.HasValue);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            var getHitVfx = worldSpaceVFXController.Spawn(playerSpawner.Current.GetHitVfx, currentEnemy.transform.position);
            services.SoundManager.SoundPlayer.Play(soundData.PlayerGetHit, false);
            getHitVfx.Activate(cancellation.Token).Forget();
            services.SoundManager.SoundPlayer.Play(soundData.MonsterDefeated, false);
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            await task.Value;
        }

        public override async UniTask Exit()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            gameSessionController.IncreaseScore();

            if (enemySpawner.Current != null && gameSessionController.CurrentStage < gameModeController.CurrentGameMode.Settings.maxStage)
            {
                signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Process));
                return;
            }

            gameSessionController.IncreaseStage();
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Transit));
            await UniTask.Yield();
        }
    }
}