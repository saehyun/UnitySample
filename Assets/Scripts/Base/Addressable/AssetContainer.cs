using Sh.Base.Addressable.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Sh.Base.Addressable
{
    public class AssetContainer<T> : IAssetContainer
    {
        private readonly Dictionary<string, T> objects = new();

        private readonly Dictionary<string, AsyncOperationHandle<IList<T>>> handles = new();

        async Task IAssetContainer.EnsureAssetAsync(string label, IList<IResourceLocation> locations)
        {
            var loadableLocations = locations
                .Where(_ => _.ResourceType == typeof(T))
                .Where(_ => objects.ContainsKey(_.PrimaryKey) == false)
                .ToArray();

            if (!loadableLocations.Any())
                return;

            try
            {
                var handle = Addressables.LoadAssetsAsync<T>(loadableLocations, null);
                var results = await handle.Task;

                if (handles.ContainsKey(label))
                    handles[label] = handle;
                else
                    handles.Add(label, handle);

                for (var i = 0; i < results.Count; i++)
                    objects[loadableLocations[i].PrimaryKey] = results[i];
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                return;
            }
        }

        void IAssetContainer.Release(string label, IList<IResourceLocation> locations)
        {
            foreach (var location in locations.Where(_ => _.ResourceType == typeof(T)))
                objects.Remove(location.PrimaryKey);

            if (handles.TryGetValue(label, out var handle))
            {
                Addressables.Release(handle);
                handles.Remove(label);
            }
        }

        object IAssetContainer.Get(string key)
        {
            var result = objects.TryGetValue(key, out var value) ? value : default;

            return result;
        }
    }
}
