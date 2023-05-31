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
    public class ExternalStockMarketApiService : IExternalStockMarketApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ExternalStockMarketApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResultModel> GetHistoricalData(string code, DateTime start, DateTime end)
        {
            var resultModel = new ResultModel { };
            var httpClient = _httpClientFactory.CreateClient("StockData");
            var dateFrom = start.ToString("yyyy-MM-dd");
            var dateTo = end.ToString("yyyy-MM-dd");
            var result = await httpClient.GetAsync($"eod?symbols={code}&date_from={dateFrom}&date_to={dateTo}");
            if (result.IsSuccessStatusCode)
            {
                var stringContent = await result.Content.ReadAsStringAsync();
                var stockData = JsonConvert.DeserializeObject<StockDataApiResultModel>(stringContent);
                resultModel.Data = stockData;                
            }
            else
                resultModel.Errors = result.ReasonPhrase;

            return resultModel;
        }

        public async Task<ResultModel> GetRealTimeData(List<string> codes)
        {
            var codeList = string.Join(",", codes);
            var resultModel = new ResultModel { };
            var httpClient = _httpClientFactory.CreateClient("StockData");          
            var result = await httpClient.GetAsync($"quote?symbols={codeList}");
            if (result.IsSuccessStatusCode)
            {
                var stringContent = await result.Content.ReadAsStringAsync();
                var stockData = JsonConvert.DeserializeObject<RealTimeStockApiResultModel>(stringContent);
                resultModel.Data = stockData;
            }
            else
                resultModel.Errors = result.ReasonPhrase;

            return resultModel;
        }
    }
}
