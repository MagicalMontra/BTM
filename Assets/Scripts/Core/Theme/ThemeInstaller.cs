using System.Collections.Generic;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ThemeInstaller : MonoInstaller<ThemeInstaller>
    {
        [Inject]
        private ShopDatabase shopDatabase;

        public override void InstallBindings()
        {
            Container.Bind<ThemeSelector>().AsSingle();
            Container.Bind<IEnumerable<ThemeData>>().FromInstance(shopDatabase.ThemeData).AsSingle();
        }
    }
}