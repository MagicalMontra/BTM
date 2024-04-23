using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.Pooling
{
    /// <summary>
    /// pool handling instances of a prefab
    /// </summary>
    public class Pool
    {
        /// <summary>
        /// is pool empty and can be removed
        /// </summary>
        public bool IsEmpty => FreeCount + RentedCount == 0;

        /// <summary>
        /// number of objects currently rented out from this pool
        /// </summary>
        public int RentedCount { get; private set; }

        /// <summary>
        /// free objects standing by in the pool
        /// </summary>
        public int FreeCount => freeObjects.Count;

        /// <summary>
        /// recent objects are stay at the back of the list
        /// older objects staying at the front are candidate for removal
        /// </summary>
        private readonly LinkedList<PoolObject> freeObjects = new LinkedList<PoolObject>();

        private PoolObject prefab;
        private bool isDestroyed;

        public Pool(PoolObject prefab)
        {
            this.prefab = prefab;
        }

        /// <summary>
        /// rent an object out of this pool
        /// </summary>
        public PoolObject Rent()
        {
            PoolObject result = Pop();
            if (result == null)
            {
                result = GameObject.Instantiate(prefab);
                result.SetPool(this);
            }

            RentedCount++;
            result.MarkRented();
            return result;
        }

        /// <summary>
        /// remove and destroy expired free objects
        /// </summary>
        public void RemoveExpired()
        {
            // check from the front of the list (older object)
            int maxIteration = freeObjects.Count;
            for (int i = 0; i < maxIteration && freeObjects.Count > 0; i++)
            {
                PoolObject obj = freeObjects.First.Value;
                if (obj.IsDestroyed || obj.IsExpired)
                {
                    freeObjects.RemoveFirst();
                    if (!obj.IsDestroyed)
                    {
                        GameObject.Destroy(obj.gameObject);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// mark this pool as destroyed
        /// object returned after this will be destroyed
        /// </summary>
        public void MarkDestroyed()
        {
            isDestroyed = true;
        }

        /// <summary>
        /// return object to the pool
        /// </summary>
        internal void Return(PoolObject obj)
        {
            RentedCount--;

            if (isDestroyed && !obj.IsDestroyed)
            {
                Debug.Log($"{obj.GetType().Name} returned to a destroyed pool, the object will be destroyed");
                GameObject.Destroy(obj);
            }
            else if (!obj.IsDestroyed)
            {
                // push to the back of the list
                freeObjects.AddLast(obj);
            }
        }

        private PoolObject Pop()
        {
            // pop out of the back of the list
            PoolObject result = null;
            if (freeObjects.Count > 0)
            {
                result = freeObjects.Last.Value;
                freeObjects.RemoveLast();
            }
            return result;
        }
    }
}