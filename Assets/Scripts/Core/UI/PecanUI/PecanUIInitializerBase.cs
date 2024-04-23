using HotPlay.PecanUI;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public abstract class PecanUIInitializerBase : IUIInitializer
    {
        internal PecanServices Services => services;
        
        [Inject]
        private PecanServices services;

        public void Initialize()
        {
            Setup();
        }

        public void Dispose()
        {
            Terminate();
        }
        
        public abstract void Setup();

        public abstract void Terminate();
    }
}