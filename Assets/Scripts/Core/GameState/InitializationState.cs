using System;
using System.Collections.Generic;
using System.Threading;
using Zenject;
using HotPlay.PecanUI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI.Gameplay;

namespace HotPlay.BoosterMath.Core
{
    public class InitializationState : StateBase, IInitializable, IDisposable
    {
        public override GameStateEnum Name => GameStateEnum.Initialization;
        
        [Inject]
        private SignalBus signalBus;

        [Inject]
        private SoundData soundData;

        [Inject]
        private QuestionTimer timer;
        
        [Inject]
        private HeartPanel heartPanel;
        
        [Inject]
        private PecanServices services;

        [Inject]
        private IEnemySpawner enemySpawner;

        [Inject]
        private GameModeController gameMode;
        
        [Inject]
        private PlayerSpawner playerSpawner;
        
        [Inject]
        private PauseController pauseController;

        [Inject]
        private AdBannerController adBannerController;

        [Inject]
        private ShopDataController shopDataController;
        
        [Inject]
        private GameModeSelectionUI selectionUIPrefab;
        
        [Inject]
        private ParallaxController parallaxController;
        
        [Inject]
        private TutorialController tutorialController;

        [Inject]
        private GameSessionController gameSessionController;

        [Inject]
        private GameModeSelectionUI.Factory gameModeUIFactory;

        private GameModeSelectionUI selectionUI;
        private CancellationTokenSource cancellation;
        private List<UniTask> tasks = new List<UniTask>();

        
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

            pauseController.SetPauseStatus(true);
            timer.Reset().Forget();
            services.GetCustomGamePlayPanel<GameplayUI>().UpdateScore("");
            services.Events.InitGameplayCurrency(0);
            selectionUI ??= gameModeUIFactory.Create(selectionUIPrefab, services.GetCustomGamePlayPanel<GameplayUI>().transform);
            adBannerController.Show();
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
            
            parallaxController.Current.MoveForSeconds(1f, cancellation.Token).Forget();
            var soundId = -1;
            await playerSpawner.Spawn(shopDataController.EquippedCharacter, cancellation.Token);
            playerSpawner.Current.Walk(cancellation.Token);
            selectionUI.Show();
            services.SoundManager.SoundPlayer.Stop(soundId);
            playerSpawner.Current.Idle(cancellation.Token);
            tutorialController.FirstTutorialPass();
            await UniTask.WaitWhile(() => gameMode.CurrentGameMode == null && !cancellation.IsCancellationRequested);

            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                selectionUI.Hide();
                pauseController.SetPauseStatus(false);
                await UniTask.Yield();
                return;
            }
            
            if (!playerSpawner.Current.IsFloating)
                soundId = services.SoundManager.SoundPlayer.Play(soundData.PlayerStep, true);
            
            playerSpawner.Current.Walk(cancellation.Token);
            selectionUI.Hide(cancellation.Token).Forget();
            pauseController.SetPauseStatus(false);
            await parallaxController.Current.MoveForSeconds(1.5f, cancellation.Token);
            services.SoundManager.SoundPlayer.Stop(soundId);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                selectionUI.Hide();
                pauseController.SetPauseStatus(false);
                await UniTask.Yield();
                return;
            }
            
            playerSpawner.Current.Idle(cancellation.Token);
            gameSessionController.Start();
            services.SoundManager.SoundPlayer.Play(soundData.MonsterSpawn, false);
            tasks.Add(enemySpawner.Spawn(cancellation.Token));
            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellation.Token);
            
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                selectionUI.Hide();
                pauseController.SetPauseStatus(false);
                await UniTask.Yield();
                return;
            }
            
            playerSpawner.Current.Idle(cancellation.Token);
            tasks.Clear();
            heartPanel.Initialize(playerSpawner.Current.MaxHealth, playerSpawner.Current.CurrentHealth);
            await UniTask.Delay(500);
        }

        public override async UniTask Exit()
        {
            if (cancellation.IsCancellationRequested)
            {
                cancellation.Dispose();
                await UniTask.Yield();
                return;
            }
            
            pauseController.SetPauseStatus(true);
            services.GetDialog<GameplayDialog>().PauseButton.interactable = true;
            services.GetDialog<GameplayDialog>().TutorialButton.interactable = true;
            signalBus?.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Process));
            await UniTask.Yield();
        }
    }
}