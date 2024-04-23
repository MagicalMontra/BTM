using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public abstract class ModeInstallerBase : MonoInstaller<ModeInstallerBase>
    {
        [SerializeField]
        private GameModeSettings settings;
        
        public override void InstallBindings()
        {
            BasicBind();
            PatternBind();
        }

        private void BasicBind()
        {
            Container.Bind<GameMode>().AsSingle();
            Container.Bind<GameModeSettings>().FromInstance(settings).AsSingle();
        }

        internal abstract void PatternBind();
    }
}