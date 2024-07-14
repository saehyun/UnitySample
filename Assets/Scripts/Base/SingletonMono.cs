using UnityEngine;

namespace Sh.Base
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool isDestroyed = false;

        protected static T instance = default;

        public static T Instance
        {
            get
            {
                if (isDestroyed)
                    return null;

                if ((object)instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (!(instance is SingletonMono<T>))
                    {
                        GameObject singletonGameObject = new GameObject(typeof(T).FullName);
                        DontDestroyOnLoad(singletonGameObject);
                        instance = singletonGameObject.AddComponent<T>();
                    }

                    (instance as SingletonMono<T>).OnCreateInstance();
                }

                return instance;
            }
        }


        protected void Destroy()
        {
            if ((object)instance == null)
                return;

            Destroy(instance.gameObject);
            instance = null;
            isDestroyed = true;
        }


        protected virtual void OnCreateInstance() { }
    }
}