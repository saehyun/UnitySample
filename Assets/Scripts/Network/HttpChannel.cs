using Sh.Network.Controller.Internal;
using Sh.Network.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sh.Network
{
    public class HttpChannel
    {
        private static readonly Dictionary<string, MethodInfo> routes = new();


        static HttpChannel()
        {
            // Mock 플랫폼 구동을 위한 작업
            var controllers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)) && !t.IsAbstract);

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttribute<RouteAttribute>() != null);

                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<RouteAttribute>();
                    routes[attribute.Path.ToLower()] = method;
                }
            }
        }


        internal async Task<TResponse> RequestAsync<TResponse>(MessageBase message) where TResponse : MessageBase
        {
            var requestAttr = (RequestAttribute)message.GetType().GetCustomAttributes()
                .FirstOrDefault(_ => _ is RequestAttribute);

            if (requestAttr == null)
                return null;

            var route = requestAttr.Route;

            if (routes.TryGetValue(route.ToLower(), out MethodInfo method))
            {
                var controllerInstance = Activator.CreateInstance(method.DeclaringType);
                var task = (Task)method.Invoke(controllerInstance, new object[] { message });

                await task.ConfigureAwait(false);

                var responseProperty = task.GetType().GetProperty("Result");
                return responseProperty.GetValue(task) as TResponse;
            }
            else
            {
                return null;
            }
        }
    }
}
