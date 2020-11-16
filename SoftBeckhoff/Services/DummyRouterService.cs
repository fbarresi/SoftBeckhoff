using Microsoft.Extensions.Logging;
using SoftBeckhoff.Interfaces;
using SoftBeckhoff.Models;
using TwinCAT.Ads.TcpRouter;

namespace SoftBeckhoff.Services
{
    internal class DummyRouterService : IRouterService
    {
        private readonly ILogger<DummyRouterService> logger;

        public DummyRouterService(ILogger<DummyRouterService> logger)
        {
            this.logger = logger;
        }
        public bool TryAddRoute(RouteSetting route)
        {
            logger.LogWarning($"Try to add Route '{route}' into dummy router service:\nrestart the software with --add-router argument or use the another local router service ");
            return false;
        }

        public RouteCollection GetRoutes()
        {
            return null;
        }
    }
}