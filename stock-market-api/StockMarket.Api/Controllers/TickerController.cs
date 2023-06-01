using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMarket.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TickerController : ControllerBase
    {
        private readonly ITickerService _tickerService;
        public TickerController(ITickerService tickerService)
        {
            _tickerService = tickerService;
        }


        [HttpPost("Historic")]
        public async Task<IActionResult> GetHistoricalData(TickerFilterModel model)
        {
            var result = await _tickerService.GetHistoricalData(model);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [HttpPost("RealTime")]        
        public async Task<IActionResult> GetRealTimeData(TickerFilterModel model)
        {
            var result = await _tickerService.GetRealTimeData(model);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }
    }
}
