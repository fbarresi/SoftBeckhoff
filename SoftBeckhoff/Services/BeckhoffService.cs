using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads.Server;

namespace SoftBeckhoff.Services
{
    public class BeckhoffService : IPlcService
    {
		private readonly CompositeDisposable disposables = new CompositeDisposable();
		private readonly ILogger logger;
		private BeckhoffServer server;
        public BeckhoffService(ILogger<BeckhoffService> logger)
        {
			this.logger = logger;

			var init = Observable.Timer(TimeSpan.FromMilliseconds(500))
			.SelectMany(_ => InitializeAsync())
			.Do(
					_ => { },
					ex => logger?.LogError(ex, "Error while initializing Beckhoff Service"))
			.Retry()
			.Subscribe();
			disposables.Add(init);
        }

        public void Dispose()
		{
			
		}

		private Task<Unit> InitializeAsync()
		{
			logger.LogInformation("Initializing Beckhoff server...");
			server = new BeckhoffServer(logger);
			return Task.FromResult(Unit.Default);
		}
    }

    public class BeckhoffServer
    {
	    private readonly ILogger logger;
	private readonly AmsServerNet server;
	    public BeckhoffServer(ILogger logger)
	    {
		    this.logger = logger;
		    server = (AmsServerNet) typeof(AmsServerNet)
			    .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, 
				    null, 
				    CallingConventions.Any, 
				    new []{typeof(ILogger)}, 
				    null)
			    ?.Invoke(new[] {logger});
			logger.LogInformation($"Beckhoff server created");
			Task.Delay(500).Wait();
		    var result = server.AmsConnect(851, "SoftPlc");
		    var connected = server.IsServerConnected;
			logger.LogInformation($"Beckhoff server connected = {connected} with result = {result}");
	    }
    }
}
