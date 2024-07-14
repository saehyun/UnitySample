using System;

namespace Sh.Base
{
    public abstract class Singleton<T>
    {
        private static T instance = default;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                    
                    (instance as Singleton<T>).OnCreateInstance();
                }

                return instance;
            }
        }

        protected virtual void OnCreateInstance() { }
    }
}
