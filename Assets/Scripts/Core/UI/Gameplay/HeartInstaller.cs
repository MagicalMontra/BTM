using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class HeartInstaller : MonoInstaller<HeartInstaller>
    {
        [SerializeField]
        private HeartIcon iconPrefab;

        [SerializeField]
        private HeartPanel panelPrefab;

        [Inject]
        private PecanServices pecanServices;
        
        public override void InstallBindings()
        {
            var disableGroup = pecanServices.GetCustomGamePlayPanel<GameplayUI>().DisableGroup;
            Container.Bind<HeartPanel>().FromComponentInNewPrefab(panelPrefab).UnderTransform(pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform).AsSingle();
            Container.BindMemoryPool<HeartIcon, HeartIcon.Pool>().WithInitialSize(3).FromComponentInNewPrefab(iconPrefab).UnderTransform(disableGroup).AsSingle();
        }
    }
}