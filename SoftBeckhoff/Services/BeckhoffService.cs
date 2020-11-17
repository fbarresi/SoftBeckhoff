using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftBeckhoff.Interfaces;
using SoftBeckhoff.Models;

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

        public IEnumerable<SymbolDto> GetSymbols()
        {
	        return server.Symbols.Select(pair => new SymbolDto(){Name = pair.Key, Type = pair.Value.Type.Name});
        }

        public byte[] GetSymbol(string name)
        {
	        return server.ReadSymbol(name);
        }

        public void SetSymbol(string name, byte[] value)
        {
	        server.WriteSymbol(name, value);
        }

        public void CreateSymbol(SymbolDto symbol)
        {
	        var symbolType = symbol.Type.StartsWith("System.") ? symbol.Type : "System." + symbol.Type;
	        var type = Type.GetType(symbolType);
	        if (type != null)
	        {
		        var adsSymbol = new AdsSymbol(symbol.Name, type);
		        server.AddSymbol(adsSymbol);
	        }
	        else
		        throw new Exception($"Unable to find type named {symbolType}");
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
