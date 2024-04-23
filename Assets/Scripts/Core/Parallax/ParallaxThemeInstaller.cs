using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxThemeInstaller : MonoInstaller<ParallaxThemeInstaller>
    {
        [SerializeField]
        private string id;
        
        [SerializeField]
        private ParallaxElement[] elements;

        public override void InstallBindings()
        {
            Container.Bind<string>().FromInstance(id).AsSingle();
            Container.Bind<ParallaxThemeController>().FromComponentOn(gameObject).AsSingle();

            foreach (var element in elements)
            {
                Container.Bind<ParallaxElement>().FromInstance(element).AsTransient();
            }
        }
    }
}