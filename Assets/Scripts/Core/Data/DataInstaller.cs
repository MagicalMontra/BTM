using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class DataInstaller : Installer<DataInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataEncoder>().To<JsonDataEncoder>().AsSingle();
            Container.Bind<IDataDecoder>().To<JsonDataDecoder>().AsSingle();

            Container.BindInterfacesAndSelfTo<ShopDataController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrencyDataController>().AsSingle();
        }
    }
}