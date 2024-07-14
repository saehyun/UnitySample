using System;

namespace Sh.Network.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RouteAttribute : Attribute
    {
        public string Path { get; private set; }


        public RouteAttribute(string path)
        {
            Path = path;
        }
    }
}
