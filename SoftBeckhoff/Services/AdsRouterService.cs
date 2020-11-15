using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads.TcpRouter;

namespace SoftBeckhoff.Services
{
    public class AdsRouterService : BackgroundService, IRouterService
    {
        private readonly ILogger<TwinCAT.Ads.AdsRouterService.RouterService> logger;
        private readonly IConfiguration configuration;
        private AmsTcpIpRouter amsTcpIpRouter;

        public AdsRouterService(ILogger<TwinCAT.Ads.AdsRouterService.RouterService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            logger.LogDebug("");
            logger.LogDebug("Application Directories");
            logger.LogDebug("=======================");
            logger.LogDebug("ApplicationPath: " + Environment.GetCommandLineArgs()[0]);
            logger.LogDebug("BaseDirectory: " + AppContext.BaseDirectory);
            logger.LogDebug("CurrentDirectory: " + Directory.GetCurrentDirectory());
            logger.LogDebug("");
            logger.LogDebug("Configuration");
            logger.LogDebug("=============");
            logger.LogDebug("ASPNETCORE_ENVIRONMENT: " + ConfigurationBinder.GetValue(configuration, "ASPNETCORE_ENVIRONMENT", "Production"));
            logger.LogDebug("");
            amsTcpIpRouter = new AmsTcpIpRouter(logger, configuration);
            amsTcpIpRouter.RouterStatusChanged += Router_RouterStatusChanged;
            
            await amsTcpIpRouter.StartAsync(cancel);
        }

        private void Router_RouterStatusChanged(object? sender, RouterStatusChangedEventArgs e)
        {
            int routerStatus = (int) e.RouterStatus;
        }

        public bool TryAddRoute(Route route)
        {
            return amsTcpIpRouter.TryAddRoute(route);
        }
    }
}