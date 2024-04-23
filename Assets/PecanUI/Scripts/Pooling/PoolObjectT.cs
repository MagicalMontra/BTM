using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.Pooling
{
    public class PoolObject<T> : PoolObject where T : PoolObject
    {
        private PoolManager manager;

        /// <summary>
        /// rent from a PoolManager in the SharedContext
        /// </summary>
        public T Rent()
        {
            if (isPoolInstance)
            {
                throw new System.Exception("Not instantiating from prefab");
            }

            return GetManager().Rent(this) as T;
        }

        /// <summary>
        /// rent from a PoolManager in the SharedContext
        /// </summary>
        public T Rent(Vector3 position, Quaternion rotation, Vector3 localScale, Transform parent)
        {
            T obj = Rent();
            obj.transform.SetParent(parent);
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.localScale = localScale;

            return obj;
        }

        private PoolManager GetManager()
        {
            if (manager == null)
            {
                manager = SharedContext.Instance.Get<PoolManager>();
                if (manager == null)
                {
                    manager = PoolManager.Create();
                    SharedContext.Instance.Add(manager);
                }
            }

            return manager;
        }
    }
}