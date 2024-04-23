namespace HotPlay.QuickMath.Utilities
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {  
        private static T instance = null;  
        public static T Instance  
        {  
            get  
            {  
                if (instance == null)  
                {  
                    instance = new T();  
                }  
                return instance;  
            }  
        }

        public static bool HasInstance()
        {
            return instance != null;
        }
    }
}