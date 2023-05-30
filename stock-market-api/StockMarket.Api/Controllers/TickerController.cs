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
    [Route("[controller]")]
    public class TickerController : ControllerBase
    {
        private readonly ITickerService _tickerService;
        public TickerController(ITickerService tickerService)
        {
            _tickerService = tickerService;
        }


        [HttpPost]
        public async Task<IActionResult> Filter(TickerFilterModel model)
        {
            var result = await _tickerService.GetHistoricalData(model.TickerCode, model.Start, model.End);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }
    }
}
