using Sh.Contents.Common;
using Sh.Network.Core;

namespace Sh.Network.Protocol
{
    public class TabData
    {
        internal TabType TabType { get; set; }

        public string TabText { get; set; }

        public string ContentText { get; set; }
    }
    
    [Request("Lobby/LobbyTabData")]
    public class ReqLobbyTabData : MessageBase
    {

    }

    public class ResLobbyTabData : MessageBase
    {
        internal TabData[] tabData;
    }
}
