﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftBeckhoff.Interfaces;
using SoftBeckhoff.Models;
using SoftBeckhoff.Services;
using TwinCAT.Ads.TcpRouter;

namespace SoftBeckhoff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoftBeckhoffController : ControllerBase
    {
        private readonly ILogger<SoftBeckhoffController> logger;
        private readonly IPlcService plcService;
        private readonly IRouterService routerService;

        public SoftBeckhoffController(ILogger<SoftBeckhoffController> logger, IPlcService plcService, IRouterService routerService)
        {
            this.logger = logger;
            this.plcService = plcService;
            this.routerService = routerService;
        }

        [HttpGet("/symbols")]
        public IEnumerable<SymbolDto> GetSymbols()
        {
            return plcService.GetSymbols();
        }
        
        [HttpGet("/symbols/{name}")]
        public byte[] GetSymbol([FromRoute]string name)
        {
            return plcService.GetSymbol(name);
        }
        
        [HttpPut("/symbols/{name}")]
        public void SetSymbol([FromRoute]string name, [FromBody]byte[] value)
        {
            plcService.SetSymbol(name, value);
        }
        
        [HttpPost("/symbols")]
        public void CreateSymbol([FromBody]SymbolDto symbol)
        {
            plcService.CreateSymbol(symbol);
        }
        
        [HttpGet("/routes")]
        public RouteCollection GetRoutes()
        {
            return routerService.GetRoutes();
        }
        
        [HttpPut("/routes")]
        public bool AddRoutes([FromBody]RouteSetting route)
        {
            return routerService.TryAddRoute(route);
        }
    }
}
