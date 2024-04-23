using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class DebugInstaller : MonoInstaller<DebugInstaller>
    {
#if CHEAT_ENABLED
        [Inject]
        private PecanServices pecanServices;

        [SerializeField]
        private MainMenuDebugPanel mainMenuDebugPrefab;

        [SerializeField]
        private GameplayDebugPanel gameplayDebugPrefab;
#endif
        
        public override void InstallBindings()
        {
#if CHEAT_ENABLED
            Container.Bind<MainMenuDebugPanel>().FromComponentInNewPrefab(mainMenuDebugPrefab).UnderTransform(pecanServices.GetDialog<MainMenuDialog>().transform).AsSingle().NonLazy();
            Container.Bind<GameplayDebugPanel>().FromComponentInNewPrefab(gameplayDebugPrefab).UnderTransform(pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform).AsSingle().NonLazy();
#endif
        }
    }
}