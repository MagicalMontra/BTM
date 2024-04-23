using System;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public abstract class BaseItemDropInstaller : MonoInstaller<BaseItemDropInstaller>
    {
        [SerializeField]
        private ItemDropTypeEnum type;
        
        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        private ItemDropBase prefab;
        
        public override void InstallBindings()
        {
            Container.Bind<Sprite>().FromInstance(sprite).AsSingle();
            Container.Bind<ItemDropBase>().FromInstance(prefab).AsSingle();
            Container.Bind<ItemDropTypeEnum>().FromInstance(type).AsSingle();
            Container.BindMemoryPool<ItemDropBase, ItemDropBase.Pool>().FromComponentInNewPrefab(prefab).UnderTransformGroup(Convert.ToString(type)).AsSingle();
        }
    }
}