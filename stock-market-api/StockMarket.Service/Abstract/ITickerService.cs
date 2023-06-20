using StockMarket.Service.Model;
using StockMarket.Service.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface ITickerService
    {
        Task<ResultModel> GetHistoricalData(HistoricTickerFilterModel model, int userId);
        Task<ResultModel> GetRealTimeData(RealTimeTickerFilterModel model, int userId);
    }
}
