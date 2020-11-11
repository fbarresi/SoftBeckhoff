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
			stream = (NetworkStream)typeof(AmsServerNet).GetTypeInfo().GetDeclaredMethod("registerCommandPipe")?.Invoke(server, new object[0]);

			disposables.Add(server);
			disposables.Add(stream);

			server.RegisterReceiver(this);
			
			var inputStream = Observable.Interval(TimeSpan.FromSeconds(1))
				.Where(_ => stream.CanRead)
				.Select(_ =>
				{
					var buffer = new byte[1024];
					var read = stream.Read(new Span<byte>(buffer));
					return buffer.Take(read);
				})
				.Select(buffer => Encoding.ASCII.GetString(buffer.ToArray()))
				.Subscribe(Console.WriteLine)
				;
			disposables.Add(inputStream);
	    }

	public void Dispose()
	{
		disposables?.Dispose();
	}

	public async Task<AdsErrorCode> OnReceivedAsync(AmsCommand frame, CancellationToken cancel)
	{
		logger.LogInformation($"{frame.Dump()}");
		//await server.AmsSendAsync(new AmsCommand(new AmsHeader(frame.Header.Sender, frame.Header.Target, AdsCommandId.ReadState, AmsStateFlags.MaskAdsResponse, 0, 0, 0), new ReadOnlyMemory<byte>()), cancel);
		return AdsErrorCode.Succeeded;
	}
    }
}
