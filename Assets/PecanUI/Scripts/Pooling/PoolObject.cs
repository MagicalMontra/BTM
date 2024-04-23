using UnityEngine;

namespace HotPlay.Pooling
{
    /// <summary>
    /// object to be pooled
    /// </summary>
    public class PoolObject : MonoBehaviour
    {
        /// <summary>
        /// is the object destroyed using GameObject.Destroy or not
        /// </summary>
        /// <value></value>
        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// is this a free and expired object or not
        /// </summary>
        internal bool IsExpired => !isRented && Time.time > expireTime;

        /// <summary>
        /// is the object currently rented
        /// </summary>
        protected bool isRented { get; private set; }

        /// <summary>
        /// indicate that this is a pool instantiated object (not original prefab)
        /// </summary>
        protected bool isPoolInstance => pool != null;

        private const string rentedNameFormat = "{0}-rented";
        private const string freeNameFormat = "{0}-free";

        [SerializeField, Tooltip("Free object will be destroyed after this duration passed")]
        private float expireDuration = 10;

        private float expireTime;
        private Pool pool;
        private string originalName;

        /// <summary>
        /// return object to its pool
        /// </summary>
        public void Return()
        {
            Debug.Assert(isRented, "returning a returned pool object");
            Debug.Assert(isPoolInstance, "returning an object without pool");
            if (!isRented || !isPoolInstance)
            {
                return;
            }

            isRented = false;
            expireTime = Time.time + expireDuration;
            pool.Return(this);

            if (!IsDestroyed)
            {
                name = string.Format(freeNameFormat, originalName);
                OnReturned();
            }
        }

        /// <summary>
        /// set the pool this object instance is belonged to
        /// </summary>
        internal void SetPool(Pool pool)
        {
            this.pool = pool;
            originalName = name;
        }

        /// <summary>
        /// mark the object as rented
        /// </summary>
        internal void MarkRented()
        {
            Debug.Assert(!isRented, "renting a rented pool object should never happen");
            Debug.Assert(!IsDestroyed, "destroyed pool object should never rented out");
            isRented = true;
            name = string.Format(rentedNameFormat, originalName);
            OnRented();
        }

        /// <summary>
        /// called when the object is rented out
        /// </summary>
        protected virtual void OnRented()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// called when the object is returned to its pool
        /// won't be called if the object is destroyed
        /// </summary>
        protected virtual void OnReturned()
        {
            gameObject.SetActive(false);

            // avoid getting destroyed together with parent object
            transform.SetParent(null);
        }

        /// <summary>
        /// if the object is destroyed but still on rent,
        /// return it so the pool have a correct count of pool objects
        /// </summary>
        protected virtual void OnDestroy()
        {
            IsDestroyed = true;
            if (isRented)
            {
                // this can be just an informative log, it is supported by the pool
                Debug.Log($"{this.GetType().Name} destroyed before return");
                Return();
            }
        }
    }
}