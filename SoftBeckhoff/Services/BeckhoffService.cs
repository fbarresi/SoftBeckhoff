using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
			disposables?.Dispose();	
		}

		private Task<Unit> InitializeAsync()
		{
			logger.LogInformation("Initializing Beckhoff server...");
			server = new BeckhoffServer(logger);
			disposables.Add(server);
			return Task.FromResult(Unit.Default);
		}
    }
}
