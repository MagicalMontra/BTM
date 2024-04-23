using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxInstaller : MonoInstaller<ParallaxInstaller>
    {
        [SerializeField]
        private ParallaxThemeController[] contexts;
        
        public override void InstallBindings()
        {
            Container.Bind<ParallaxController>().AsSingle();
            Container.Bind<IEnumerable<ParallaxThemeController>>().FromInstance(contexts).AsSingle();
            Container.BindFactory<Object, ParallaxThemeController, ParallaxThemeController.Factory>().FromFactory<PrefabFactory<ParallaxThemeController>>();
        }
    }
}