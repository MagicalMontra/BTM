using System;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class RewindItemDropInstaller : BaseItemDropInstaller
    {
        [SerializeField]
        private float duration;

        private const string durationId = "Duration";
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind(typeof(ITimer), typeof(IInitializable), typeof(IDisposable), typeof(ITickable)).To<RewindActiveTimer>().AsSingle();
            Container.Bind<IItemDropWorker>().To<RewindItemDropWorker>().AsSingle();
            Container.Bind<float>().WithId(durationId).FromInstance(duration).AsTransient();
        }
    }
}