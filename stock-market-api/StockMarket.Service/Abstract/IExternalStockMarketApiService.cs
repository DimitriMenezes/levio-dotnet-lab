using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface IExternalStockMarketApiService
    {
        Task<ResultModel> GetHistoricalData(string code, DateTime start, DateTime end);
        Task<ResultModel> GetRealTimeData(List<string> codes);
    }
}
