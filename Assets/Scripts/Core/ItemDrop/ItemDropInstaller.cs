using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ItemDropInstaller : MonoInstaller<ItemDropInstaller>
    {
        [SerializeField] 
        private BaseItemDropInstaller[] contexts;

        private readonly IEnumerable<IItemDropWorker> dropAmountWorkers;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemDropController>().AsSingle();

            foreach (var context in contexts)
            {
                Container.Bind<IItemDropWorker>().FromSubContainerResolve().ByNewContextPrefab(context).AsCached();
            }
        }
    }
}