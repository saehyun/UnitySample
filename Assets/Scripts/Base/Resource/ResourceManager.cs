using Sh.Base.Addressable;
using System.Collections.Generic;
using UnityEngine;

namespace Sh.Base.Resources
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public T Instantiate<T>(string key, Transform parent) where T : Component
        {
            var prefab = GetPrefab(key);
            var instance = Object.Instantiate(prefab);

            instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            return instance.GetComponent<T>();
        }

        public GameObject GetPrefab(string key)
        {
            try
            {
                return AddressablePreLoader.Instance.Get<GameObject>(key);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
