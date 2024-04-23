using System;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    [Serializable]
    public class GlobalWorldVFXSettings
    {
        public WorldSpaceVFXBase playerTimeOutDeathVfx;
    }
    
    public class WorldSpaceVFXInstaller : MonoInstaller<WorldSpaceVFXInstaller>
    {
        [SerializeField]
        private GlobalWorldVFXSettings globalWorldVFXSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WorldSpaceVFXController>().AsSingle();
            Container.Bind<GlobalWorldVFXSettings>().FromInstance(globalWorldVFXSettings).AsSingle();
            Container.BindFactory<WorldSpaceVFXBase, Vector3, Transform, WorldSpaceVFXBase, WorldSpaceVFXBase.Factory>().FromFactory<WorldSpaceVFXFactory>();
        }
    }
}