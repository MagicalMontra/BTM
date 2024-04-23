using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ShadowwInstaller : MonoInstaller<ShadowwInstaller>
    {
        [SerializeField]
        private Shadow prefab;

        [SerializeField]
        private Transform disableGroup;
        
        [SerializeField]
        private ShadowConfig shadowConfig;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShadowGenerator>().AsSingle();
            Container.Bind<ShadowConfig>().FromInstance(shadowConfig).AsSingle().NonLazy();
            Container.BindMemoryPool<Shadow, Shadow.Pool>().WithInitialSize(3).FromComponentInNewPrefab(prefab).UnderTransform(disableGroup).AsSingle();
        }
    }
}