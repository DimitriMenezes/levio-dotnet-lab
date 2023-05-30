using Newtonsoft.Json;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Concrete
{
    public class TickerService : ITickerService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TickerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task GetRealTimeData(string code)
        {

        }

        public async Task<ResultModel> GetHistoricalData(string code, DateTime begin, DateTime end)
        {
            var httpClient = _httpClientFactory.CreateClient("StockData");

            var dateFrom = begin.ToString("yyyy-MM-dd");
            var dateTo= end.ToString("yyyy-MM-dd");
            var result = await httpClient.GetAsync($"eod?symbols={code}&date_from={dateFrom}&date_to={dateTo}");
            if (result.IsSuccessStatusCode)
            {
                var stringContent = await result.Content.ReadAsStringAsync();
                var stockData = JsonConvert.DeserializeObject<StockDataApiResultModel>(stringContent);
                return new ResultModel { Data = stockData.Data };
            }
            else
            {
                return new ResultModel { Errors = result.ReasonPhrase };
            }
        }
    }
}
