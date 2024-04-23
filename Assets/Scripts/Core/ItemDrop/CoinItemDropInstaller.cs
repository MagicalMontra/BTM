namespace HotPlay.BoosterMath.Core
{
    public class CoinItemDropInstaller : BaseItemDropInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IItemDropWorker>().To<CoinItemDropWorker>().AsSingle();
        }
    }
}