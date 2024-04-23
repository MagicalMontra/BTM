using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class TimerInstaller : MonoInstaller<TimerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<QuestionTimer>().AsSingle();
        }
    }
}