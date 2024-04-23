using System;
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
    public class ReinitializationState : StateBase
    {
        public override GameStateEnum Name => GameStateEnum.Reinitialization;
        
        [Inject]
        private SignalBus signalBus;
        
        [Inject]
        private readonly HeartPanel heartPanel;
        
        [Inject]
        private readonly GameModeController gameMode;
        
        [Inject]
        private readonly PecanServices services;
        
        [Inject]
        private readonly IEnemySpawner enemySpawner;

        [Inject]
        private readonly PlayerSpawner playerSpawner;
                
        [Inject]
        private readonly PauseController pauseController;
        
        [Inject]
        private readonly GameplayUIController gameplayUI;

        [Inject]
        private readonly AdBannerController adBannerController;

        [Inject]
        private readonly ItemDropController itemDropController;
        
        [Inject]
        private readonly GameSessionController gameSessionController;
        
        [Inject]
        private readonly WorldSpaceVFXController worldSpaceVFXController;
        
        public override async UniTask Enter()
        {
            pauseController.SetPauseStatus(false);
            services.GetDialog<GameplayDialog>().SetCanvasGroupInteractable(false);
            await UniTask.Yield();
        }

        public override async UniTask Execute()
        {
            gameplayUI.Hide();
            worldSpaceVFXController.Dispose();
            itemDropController.DisposeDrops();
            heartPanel.Dispose();
            gameMode.DeselectMode();
            enemySpawner.Despawn();
            playerSpawner.Despawn();
            adBannerController.Hide();
            gameSessionController.Start();
            await UniTask.Yield();
        }

        public override async UniTask Exit()
        {
            services.GetDialog<GameplayDialog>().SetCanvasGroupInteractable(true);
            signalBus?.AbstractFire(new ChangeGameStateSignal(GameStateEnum.Initialization));
            await UniTask.Yield();
        }
    }
}