using System;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads.Server;

namespace SoftBeckhoff.Services
{
    public class BeckhoffService : IPlcService
    {
        public BeckhoffService(ILogger<BeckhoffService> logger)
        {
	        var server = new BeckhoffServer(logger);
        }

        public void Dispose()
		{
			
		}   
    }

    public class BeckhoffServer
    {
	    private readonly ILogger _logger;

	    public BeckhoffServer(ILogger logger)
	    {
		    _logger = logger;
		    var server = (AmsServerNet) typeof(AmsServerNet)
			    .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, 
				    null, 
				    CallingConventions.Any, 
				    new []{typeof(ILogger)}, 
				    null)
			    ?.Invoke(new[] {logger});
		    var result = server.AmsConnect(851, "SoftPlc");
		    var connected = server.IsServerConnected;
	    }
    }
}