using System;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class SlowItemDropInstaller : BaseItemDropInstaller
    {
        [SerializeField]
        private float duration = 25f;
        
        [SerializeField]
        private float multiplier = 0.3f;

        private const string durationId = "Duration";
        private const string multiplierId = "Multiplier";
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind(typeof(ITimer), typeof(IInitializable), typeof(IDisposable), typeof(ITickable)).To<SlowActiveTimer>().AsSingle();
            Container.Bind<IItemDropWorker>().To<SlowItemDropWorker>().AsSingle();
            Container.Bind<float>().WithId(durationId).FromInstance(duration).AsTransient();
            Container.Bind<float>().WithId(multiplierId).FromInstance(multiplier).AsTransient();
        }
    }
}