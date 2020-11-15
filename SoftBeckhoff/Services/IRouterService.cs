using Microsoft.Extensions.Logging;
using TwinCAT.Ads.TcpRouter;

namespace SoftBeckhoff.Services
{
    public interface IRouterService
    {
        bool TryAddRoute(Route route);
        RouteCollection GetRoutes();
    }

    internal class DummyRouterService : IRouterService
    {
        private readonly ILogger<DummyRouterService> logger;

        public DummyRouterService(ILogger<DummyRouterService> logger)
        {
            this.logger = logger;
        }
        public bool TryAddRoute(Route route)
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