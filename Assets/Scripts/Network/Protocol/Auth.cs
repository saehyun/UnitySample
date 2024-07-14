using Sh.Network.Core;

namespace Sh.Network.Protocol
{
    [Request("Auth/RegisterGuest")]
    public class ReqRegisterGuest : MessageBase
    {
    }

    public class ResRegisterGuest : MessageBase
    {
        public string AuthToken { get; set; }
    }
}
