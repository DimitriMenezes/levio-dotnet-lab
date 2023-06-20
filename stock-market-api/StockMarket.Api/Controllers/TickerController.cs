using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model.Filter;
using System.Threading.Tasks;

namespace StockMarket.Api.Controllers
{
    
    [Authorize]
    [Route("[controller]")]
    public class TickerController : BaseController
    {
        private readonly ITickerService _tickerService;        
        public TickerController(ITickerService tickerService)
        {
            _tickerService = tickerService;            
        }


        [HttpPost("Historic")]
        public async Task<IActionResult> GetHistoricalData(HistoricTickerFilterModel model)
        {
            var userId = GetLoggedUserId();
            var result = await _tickerService.GetHistoricalData(model, userId);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [HttpPost("RealTime")]        
        public async Task<IActionResult> GetRealTimeData(RealTimeTickerFilterModel model)
        {
            var userId = GetLoggedUserId();
            var result = await _tickerService.GetRealTimeData(model, userId);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }
    }
}
