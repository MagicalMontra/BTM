namespace HotPlay.BoosterMath.Core.Enemy
{
    public class MultipleEnemyInstaller : EnemyInstaller
    {
        protected override void BindSelector()
        {
            Container.Bind<IEnemySpawner>().To<MultipleEnemySpawner>().AsSingle();
        }
    }
}