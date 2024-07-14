using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Sh.Base.Addressable.Interface
{
    public interface IAssetContainer
    {
        Task EnsureAssetAsync(string label, IList<IResourceLocation> locations);

        void Release(string label, IList<IResourceLocation> locations);

        object Get(string key);
    }
}
