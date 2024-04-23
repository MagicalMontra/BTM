using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class SoundStatusInstaller : MonoInstaller<SoundStatusInstaller>
    {
        private const string statusId = "SoundStatus";
        
        public override void InstallBindings()
        {
            Container.Bind<SoundStatus>().AsSingle();
            Container.Bind<string>().WithId(statusId).FromInstance(statusId).AsTransient();
        }
    }
}