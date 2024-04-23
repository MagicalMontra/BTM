namespace HotPlay.BoosterMath.Core.Enemy
{
    public class SingleEnemyInstaller : EnemyInstaller
    {
        protected override void BindSelector()
        {
            Container.Bind<IEnemySpawner>().To<SingleEnemySpawner>().AsSingle();
        }
    }
}