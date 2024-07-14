using System;

namespace Sh.Network.Controller.Internal
{
    public abstract class ControllerBase
    {
        protected static void Assert(bool condition, string message)
        {
            if (!condition)
                throw new Exception(message);
        }
    }
}
