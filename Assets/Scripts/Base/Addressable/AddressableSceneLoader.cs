using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Sh.Base.Addressable
{
    public class AddressableSceneLoader : SingletonMono<AddressableSceneLoader>
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> sceneHandles = new();

        public async Task LoadSceneAsync(string scene)
        {
            await AddressablePreLoader.Instance.EnsureAsync(scene);

            if (sceneHandles.TryGetValue(scene, out var sceneHandle))
            {
                await sceneHandle.Task;
            }
            else
            {
                sceneHandle = Addressables.LoadSceneAsync(scene);

                await sceneHandle.Task;
            }
        }

        
        public async Task UnloadSceneAsync(string scene)
        {
            if (!sceneHandles.TryGetValue(scene, out var sceneHandle))
                return;

            if (sceneHandle.Result.Scene.isLoaded)
                return;

            var unloadHandle = Addressables.UnloadSceneAsync(sceneHandle, UnloadSceneOptions.None);

            await unloadHandle.Task;
            await AddressablePreLoader.Instance.ReleaseAsync(scene);

            sceneHandles.Remove(scene);
        }
    }
}