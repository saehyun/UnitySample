using Sh.Network.Controller.Internal;
using Sh.Network.Core;
using Sh.Network.Protocol;
using System.Threading.Tasks;

namespace Sh.Network.Controller
{
    public class AuthController : ControllerBase
    {
        [Route("Auth/RegisterGuest")]
        public async Task<ResRegisterGuest> OnRegisterGuest(ReqRegisterGuest request)
        {
            

            return new ResRegisterGuest { AuthToken = "1" };
        }
    }
}
