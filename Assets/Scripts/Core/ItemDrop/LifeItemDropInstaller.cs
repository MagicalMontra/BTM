namespace HotPlay.BoosterMath.Core
{
    public class LifeItemDropInstaller : BaseItemDropInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IItemDropWorker>().To<LifeItemDropWorker>().AsSingle();
        }
    }
}