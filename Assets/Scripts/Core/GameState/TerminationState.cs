using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using MoreMountains.Feedbacks;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class TerminationState : StateBase
    {
        public override GameStateEnum Name => GameStateEnum.Termination;
        
        [Inject]
        private readonly SoundData soundData;
        
        [Inject]
        private readonly HeartPanel heartPanel;
        
        [Inject]
        private readonly PecanServices services;

        [Inject]
        private readonly IEnemySpawner enemySpawner;

        [Inject]
        private readonly PauseController pauseController;

        [Inject]
        private readonly GameplayUIController gameplayUI;
        
        [Inject]
        private readonly QuestionController questionController;
        
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
            await UniTask.CompletedTask;
        }

        public override async UniTask Execute()
        {
            gameSessionController.End();
            services.GetDialog<ResultDialog>().SetCanvasGroupInteractable(false);
            services.SoundManager.SoundPlayer.Play(soundData.GameplayResult, false);
            await UniTask.Yield();
        }

        public override async UniTask Exit()
        {
            await UniTask.WaitUntil(() => services.GetDialog<ResultDialog>().State == VisibilityState.Visible);
            await UniTask.DelayFrame(10);
            services.GetDialog<ResultDialog>().SetCanvasGroupInteractable(true);
            worldSpaceVFXController.Dispose();
            itemDropController.DisposeDrops();
            adBannerController.Hide();
            heartPanel.Dispose();
            enemySpawner.Despawn();
            services.SoundManager.SoundPlayer.StopBGM();
        }
    }
}