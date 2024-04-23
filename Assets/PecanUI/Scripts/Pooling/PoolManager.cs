using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.Pooling
{
    /// <summary>
    /// manager managing pools
    /// new pool will be created for each prefab
    /// empty pools are preriodically removed
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        private const float cleanInterval = 1;
        private readonly Dictionary<PoolObject, Pool> poolMap = new Dictionary<PoolObject, Pool>();
        private float nextClean;

        public static PoolManager Create()
        {
            GameObject gameObject = new GameObject(nameof(PoolManager));
            Object.DontDestroyOnLoad(gameObject);
            return gameObject.AddComponent<PoolManager>();
        }

        /// <summary>
        /// rent object of specific prefab
        /// pool will be created if not exists
        /// </summary>
        public T Rent<T>(T prefab) where T : PoolObject
        {
            if (!poolMap.TryGetValue(prefab, out Pool pool))
            {
                pool = new Pool(prefab);
                poolMap.Add(prefab, pool);
            }

            return pool.Rent() as T;
        }

        private void Update()
        {
            if (Time.time > nextClean)
            {
                nextClean = Time.time + cleanInterval;

                PoolObject emptyKey = null;

                // remove expired free objects
                foreach (var pair in poolMap)
                {
                    if (pair.Value.IsEmpty)
                    {
                        emptyKey = pair.Key;
                    }
                    else
                    {
                        pair.Value.RemoveExpired();
                    }
                }

                // remove empty pool
                // only one empty pool will be removed for each update but we aren't in hurry
                if (emptyKey != null)
                {
                    poolMap[emptyKey].MarkDestroyed();
                    poolMap.Remove(emptyKey);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (Pool pool in poolMap.Values)
            {
                pool.MarkDestroyed();
            }
        }
    }
}