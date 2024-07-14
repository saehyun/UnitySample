using Sh.Base.Addressable.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sh.Base.Addressable
{
    public class AddressablePreLoader : Singleton<AddressablePreLoader>
    {
        private readonly Dictionary<Type, IAssetContainer> assetContainers = new()
        {
            { typeof(GameObject), new AssetContainer<GameObject>() },
        };


        public async Task EnsureAsync(string label)
        {
            var locationsTask = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsTask.Task;
            var tasks = assetContainers.Values
                .Select(_ => _.EnsureAssetAsync(label, locations))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        public async Task ReleaseAsync(string label)
        {
            var locationTask = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationTask.Task;

            foreach (var container in assetContainers.Values)
                container.Release(label, locations);
        }

        public T Get<T>(string key) where T : class
        {
            if (assetContainers.TryGetValue(typeof(T), out var container) == false)
                return null;

            return container.Get(key) as T;
        }
    }
}
