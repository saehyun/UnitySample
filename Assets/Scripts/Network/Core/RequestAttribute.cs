using System;

namespace Sh.Network.Core
{
    public sealed class RequestAttribute : Attribute
    {
        public string Route { get; private set; }

        public RequestAttribute(string route)
        {
            Route = route;
        }
    }
}
