using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class WorldSpaceVFXFactory : IFactory<WorldSpaceVFXBase, Vector3, Transform, WorldSpaceVFXBase>
    {
        private DiContainer container;

        public WorldSpaceVFXFactory(DiContainer container)
        {
            this.container = container;
        }
        
        public WorldSpaceVFXBase Create(WorldSpaceVFXBase prefab, Vector3 position, Transform parent = null)
        {
            return parent == null ? Create(prefab, position) : container.InstantiatePrefabForComponent<WorldSpaceVFXBase>(prefab, position, Quaternion.identity, parent);
        }

        private WorldSpaceVFXBase Create(WorldSpaceVFXBase prefab, Vector3 position)
        {
            var instance = container.InstantiatePrefabForComponent<WorldSpaceVFXBase>(prefab);
            instance.transform.position = position;
            return instance;
        }
    }
}