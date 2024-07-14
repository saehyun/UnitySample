using Sh.Contents.Common;
using Sh.Network.Controller.Internal;
using Sh.Network.Core;
using Sh.Network.Protocol;
using System.Threading.Tasks;

namespace Sh.Network.Controller
{
    public class LobbyController : ControllerBase
    {
        [Route("Lobby/LobbyTabData")]
        public Task<ResLobbyTabData> OnLobbyTabData(ReqLobbyTabData request)
        {
            var response = new ResLobbyTabData
            {
                tabData = new[]
                {
                    new TabData
                    {
                        TabType = TabType.First,
                        TabText = "Tab 1",
                        ContentText = "Content 1"
                    },
                    new TabData
                    {
                        TabType = TabType.Second,
                        TabText = "Tab 2",
                        ContentText = "Content 2"
                    },
                    new TabData
                    {
                        TabType = TabType.Third,
                        TabText = "Tab 3",
                        ContentText = "Content 3"
                    },
                }
            };

            return Task.FromResult(response);
        }
    }
}
