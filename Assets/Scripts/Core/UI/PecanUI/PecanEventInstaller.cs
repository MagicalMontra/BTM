using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PecanEventInstaller : MonoInstaller<PecanEventInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PecanShopUIInitializer>().AsCached();
            Container.BindInterfacesAndSelfTo<PecanDailyUIInitializer>().AsCached();
            Container.BindInterfacesAndSelfTo<PecanSettingUIInitializer>().AsCached();
            Container.BindInterfacesAndSelfTo<PecanMainMenuUIInitializer>().AsCached();
            Container.BindInterfacesAndSelfTo<PecanGameplayUIInitializer>().AsCached();
        }
    }
}