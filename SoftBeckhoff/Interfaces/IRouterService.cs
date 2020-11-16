using SoftBeckhoff.Models;
using TwinCAT.Ads.TcpRouter;

namespace SoftBeckhoff.Interfaces
{
    public interface IRouterService
    {
        bool TryAddRoute(RouteSetting route);
        RouteCollection GetRoutes();
    }
}