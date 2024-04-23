using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class AdBannerInstaller : MonoInstaller<AdBannerInstaller>
    {
        [SerializeField]
        private Transform spawnPoint;

        private const string spawnPointId = "AdSpawnPoint";
        
        public override void InstallBindings()
        {
            Container.Bind<AdBannerController>().AsSingle();
            Container.Bind<Transform>().WithId(spawnPointId).FromInstance(spawnPoint).AsTransient();
            Container.BindFactory<AdBanner,  AdBanner, AdBanner.Factory>().FromFactory<PrefabFactory<AdBanner>>();
        }
    }
}