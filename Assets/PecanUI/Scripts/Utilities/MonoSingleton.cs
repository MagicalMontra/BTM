using UnityEngine;

namespace HotPlay.Utilities
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        public bool DestroyOnGameRestart = true;

        public abstract void Destroy();
    }

    public class MonoSingleton<T> : MonoSingleton where T : MonoBehaviour
    {
        private static bool isQuitting = false;

        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (isQuitting)
                {
                    return null;
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.Log($"[{typeof(T).Name}] More than 1 singleton detected");
                        return instance;
                    }

                    if (instance == null)
                    {
                        Debug.Log($"[{typeof(T).Name}] Instantiate new instance");
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = instance.GetType().Name;
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public static bool HasInstance { get { return instance != null; } }

        /// <summary>
        /// Child class must always call base.Awake in case of overriding
        /// this will handle duplication check
        /// </summary>
        public virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.Log($"[{typeof(T).Name}] Instance already existed, new instance will be destroyed");
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
            isQuitting = false;
        }

        /// <summary>
        /// This mark quitting to prevent creating new instance when stopping the game
        /// </summary>
        public virtual void OnDestroy()
        {
            Debug.Log($"[{typeof(T).Name}] Instance destroyed");
            if (instance == this)
            {
                instance = null;
                isQuitting = true;
            }
        }

        public override void Destroy()
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}