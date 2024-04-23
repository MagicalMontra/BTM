using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class WorldSpaceVFXController
    {
        [Inject]
        private WorldSpaceVFXBase.Factory factory;
        
        private List<WorldSpaceVFXBase> actives = new List<WorldSpaceVFXBase>();

        public WorldSpaceVFXBase Spawn(WorldSpaceVFXBase prefab, Vector3 position, Transform parent = null)
        {
            var instance = factory.Create(prefab, position, parent);
            actives.Add(instance);
            return instance;
        }

        public void Dispose()
        {
            foreach (var vfx in actives)
            {
                vfx.Deactivate();
            }
            
            actives.Clear();
        }
    }
}