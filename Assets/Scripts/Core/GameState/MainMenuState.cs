using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.PecanUI.Gameplay;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class MainMenuState : StateBase
    {
        public override GameStateEnum Name => GameStateEnum.MainMenu;

        [Inject]
        private SoundData soundData;
        
        [Inject]
        private HeartPanel heartPanel;
        
        [Inject]
        private ITimer questionTimer;
        
        [Inject]
        private PecanServices services;
        
        [Inject]
        private IEnemySpawner enemySpawner;

        [Inject]
        private PlayerSpawner playerSpawner;
        
        [Inject]
        private GameModeController gameMode;
        
        [Inject]
        private PauseController pauseController;

        [Inject]
        private GameplayUIController gameplayUI;

        [Inject]
        private ItemDropController itemDropController;
        
        [Inject]
        private QuestionController questionController;
        
        [Inject]
        private AdBannerController adBannerController;
        
        public override async UniTask Enter()
        {
            services.SoundManager.SoundPlayer.PlayBGM(soundData.MainMenuBGM);
            pauseController.SetPauseStatus(false);
            questionTimer.Stop();
            heartPanel.Dispose();
            enemySpawner.Despawn();
            playerSpawner.Despawn();
            itemDropController.DisposeDrops();
            adBannerController.Hide();
            gameplayUI.Hide();
            await UniTask.Yield();
        }

        public override async UniTask Execute()
        {
            gameMode.DeselectMode();
            playerSpawner.Despawn();
            enemySpawner.Despawn().Forget();
            await UniTask.Yield();
        }

        public override async UniTask Exit()
        {
            await UniTask.Yield();
        }
    }
}