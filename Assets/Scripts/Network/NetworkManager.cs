using Sh.Base;
using Sh.Network.Protocol;
using System.Threading.Tasks;

namespace Sh.Network
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        public HttpChannel HttpChannel { get; private set; }

        protected override void OnCreateInstance()
        {
            base.OnCreateInstance();

            HttpChannel = new();
        }

        public async Task<ResRegisterGuest> RegisterGuest(ReqRegisterGuest request) => await HttpChannel.RequestAsync<ResRegisterGuest>(request);

        public async Task<ResLobbyTabData> LobbyTabDataAsync(ReqLobbyTabData request) => await HttpChannel.RequestAsync<ResLobbyTabData>(request);
    }
}
