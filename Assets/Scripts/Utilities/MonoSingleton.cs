using UnityEngine;

namespace HotPlay
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool isQuitting = false;

        private static T instance = null;
        public static T Instance
        {
            get
            {
                if(isQuitting)
                {
                    return null;
                }

                if(instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();

                    if(FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return instance;
                    }

                    if(instance == null)
                    {
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
            if(instance != null && instance != this)
            {
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
            if(instance == this)
            {
                instance = null;
                isQuitting = true;
            }
        }

        // public void Destroy()
        // {
        //     instance = null;

        //     Destroy(gameObject);
        // }
    }
}
