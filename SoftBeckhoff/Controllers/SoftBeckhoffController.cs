using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SoftBeckhoff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoftBeckhoffController : ControllerBase
    {
        private readonly ILogger<SoftBeckhoffController> logger;
        private readonly IPlcService plcService;

        public SoftBeckhoffController(ILogger<SoftBeckhoffController> logger, IPlcService plcService)
        {
            this.logger = logger;
            this.plcService = plcService;
        }

        [HttpGet("/symbols")]
        public IEnumerable<object> GetSymbols()
        {
            return plcService.GetSymbols();
        }
        
        [HttpGet("/symbols/{name}")]
        public object GetSymbol([FromRoute]string name)
        {
            return plcService.GetSymbol(name);
        }
        
        [HttpPut("/symbols/{name}")]
        public void SetSymbol([FromRoute]string name, [FromBody]object value)
        {
            plcService.SetSymbol(name, value);
        }
        
        [HttpPost("/symbols")]
        public void CreateSymbol([FromBody]object symbol)
        {
            plcService.CreateSymbol(symbol);
        }
    }
}
