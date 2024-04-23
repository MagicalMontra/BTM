using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class EnemyState : StateBase, IInitializable, IDisposable
    {
        public override GameStateEnum Name => GameStateEnum.AnimateEnemy;
        
        [Inject]
        private SignalBus signalBus;

        [Inject]
        private SoundData soundData;

        [Inject]
        private HeartPanel heartPanel;
        
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
        private WorldSpaceVFXController worldSpaceVFXController;

        private Vector3 enemyPos;
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
            
            Assert.IsNotNull(enemySpawner.Current);
            services.SoundManager.SoundPlayer.Play(soundData.MonsterAttack, false);
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
            
            Assert.IsNotNull(enemySpawner.Current);
            
            if (enemySpawner.Current.AttackVfx != null)
            {
                var attackVfx = worldSpaceVFXController.Spawn(enemySpawner.Current.AttackVfx,  enemySpawner.Current.transform.position);
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
            
            await enemySpawner.Current.Attack(cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                enemySpawner.Current.Idle(cancellation.Token);
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
            enemySpawner.Current.Idle(cancellation.Token);
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                playerSpawner.Current.Idle(cancellation.Token);
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            var task = playerSpawner.Current?.GetHit(playerSpawner.Current.AttackPower, cancellation.Token);
            Assert.IsTrue(task.HasValue);
            var getHitVfx = worldSpaceVFXController.Spawn(enemySpawner.Current.GetHitVfx, playerSpawner.Current.transform.position);
            getHitVfx.Activate(cancellation.Token).Forget();
            services.SoundManager.SoundPlayer.Play(playerSpawner.Current.CurrentHealth > 0 ? soundData.PlayerGetHit : soundData.PlayerDefeated, false);
            
            await UniTask.WaitWhile(() => pauseController.IsPause, PlayerLoopTiming.Update, cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            await task.Value;
            
            if (cancellation.IsCancellationRequested)
            {
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

            heartPanel.UpdateHeart(playerSpawner.Current.CurrentHealth);
        }

        public override async UniTask Exit()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            if (playerSpawner.Current?.CurrentHealth <= 0)
            {
                signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Termination));
                return;
            }
            
            Assert.IsNotNull(enemySpawner.Current);
            enemySpawner.Current.Idle(cancellation.Token);
            signalBus.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Process));
            await UniTask.Yield();
        }
    }
}