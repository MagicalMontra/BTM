using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class BoosterUIInstaller : MonoInstaller<BoosterUIInstaller>
    {
        [Inject]
        private PecanServices services;
        
        [SerializeField]
        private BoosterPanel panelPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<BoosterPanel>().FromComponentInNewPrefab(panelPrefab).UnderTransform(services.GetCustomGamePlayPanel<GameplayUI>().transform).AsSingle();
        }
    }
}