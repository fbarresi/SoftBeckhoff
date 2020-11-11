using System;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.Ams;

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

    public class BeckhoffServer : IDisposable, IAmsFrameReceiver
    {
	    private readonly ILogger logger;
		private readonly AmsServerNet server;
		private NetworkStream stream;
		private readonly CompositeDisposable disposables = new CompositeDisposable();

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
		    var result = server.AmsConnect(852, "SoftPlc");
		    var connected = server.IsServerConnected;
			logger.LogInformation($"Beckhoff server connected = {connected} with result = {result}");

			disposables.Add(server);

			server.RegisterReceiver(this);
	    }

	public void Dispose()
	{
		disposables?.Dispose();
	}

	public async Task<AdsErrorCode> OnReceivedAsync(AmsCommand frame, CancellationToken cancel)
	{
		logger.LogInformation($"{frame.Dump()}");
		var result = server.AmsSendSync(new AmsCommand(new AmsHeader(frame.Header.Sender, frame.Header.Target, frame.Header.CommandId, AmsStateFlags.MaskAdsResponse, 8, 0, frame.Header.HUser), new ReadOnlyMemory<byte>(new byte[]{0,0,0,0,5,0,0,0})));
		return AdsErrorCode.NoError;
	}
    }
}
