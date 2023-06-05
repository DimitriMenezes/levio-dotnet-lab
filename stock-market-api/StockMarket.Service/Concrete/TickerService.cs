using AutoMapper;
using Newtonsoft.Json;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using StockMarket.Service.Model.Filter;
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
        private readonly IEntrepriseRepository _entrepriseRepository;
        private readonly IRequestLogRepository _requestLogRepository;
        private readonly IHistoricalTickerRepository _historicalTickerRepository;
        private readonly IRealTimeTickerRepository _realTimeTickerRepository;
        private readonly IRequestLogTickerRepository _requestLogTickerRepository;
        private readonly IExternalStockMarketApiService _externalApiService;
        private readonly IMapper _mapper;

        public TickerService(
            IEntrepriseRepository entrepriseRepository,
            IRequestLogRepository requestLogRepository,
            IHistoricalTickerRepository historicalTickerRepository,
            IRequestLogTickerRepository requestLogTickerRepository,
            IRealTimeTickerRepository realTimeTickerRepository,
            IExternalStockMarketApiService externalApiService,
            IMapper mapper)
        {
            _entrepriseRepository = entrepriseRepository;
            _requestLogRepository = requestLogRepository;
            _historicalTickerRepository = historicalTickerRepository;
            _realTimeTickerRepository = realTimeTickerRepository;
            _requestLogTickerRepository = requestLogTickerRepository;
            _externalApiService = externalApiService;
            _mapper = mapper;
        }


        public async Task<ResultModel> GetRealTimeData(TickerFilterModel model, int userId)
        {            
            var realTimeData = await _externalApiService.GetRealTimeData(model.Entreprises);
            if (realTimeData.Data != null)
            {
                var stockData = realTimeData.Data as RealTimeStockApiResultModel;
                var newLog = new RequestLog
                {
                    RequestJson = JsonConvert.SerializeObject(model),
                    ResponseJson = JsonConvert.SerializeObject(realTimeData.Data),
                    Status = "Success",
                    UserId = userId
                };

                await _requestLogRepository.Insert(newLog);
                var entreprises = await _entrepriseRepository.GetByCodeList(model.Entreprises);

                foreach (var data in stockData.Data)
                {
                    int currentTickerId = 0;             
                    var newTicker = new RealTimeTicker
                    {
                        EntrepriseId = entreprises.FirstOrDefault(i => i.Code == data.Ticker).Id,
                        High = data.High,                            
                        Low = data.Low,
                        Open = data.Open,
                        ReferenceDate = data.Date,
                        Current = data.Current
                    };

                    await _realTimeTickerRepository.Insert(newTicker);
                    currentTickerId = newTicker.Id;
                    
                    var newItemLog = new RequestLogTicker
                    {
                        TickerId = currentTickerId,
                        RequestLogId = newLog.Id
                    };

                    await _requestLogTickerRepository.Insert(newItemLog);
                }

                return new ResultModel { Data = stockData.Data };
            }
            else
            {
                var newLog = new RequestLog
                {
                    RequestJson = JsonConvert.SerializeObject(model),
                    ResponseJson = realTimeData.Errors as string,
                    Status = "Fail",
                    UserId = userId
                };

                await _requestLogRepository.Insert(newLog);
                return new ResultModel { Errors = realTimeData.Errors as string };
            }
        }

        public async Task<ResultModel> GetHistoricalData(TickerFilterModel model, int userId)
        {
            var entreprise = await _entrepriseRepository.GetByCode(model.Entreprises.FirstOrDefault());
            var historicalData = await _externalApiService.GetHistoricalData(model.Entreprises.FirstOrDefault(), model.Start, model.End);
            if (historicalData.Data != null)
            {
                var stockData = historicalData.Data as StockDataApiResultModel;
                var newLog = new RequestLog
                {
                    RequestJson = JsonConvert.SerializeObject(model),
                    ResponseJson = JsonConvert.SerializeObject(historicalData.Data),
                    Status = "Success",
                    UserId = userId
                };

                await _requestLogRepository.Insert(newLog);

                foreach (var data in stockData.Data)
                {
                    var entrepriseId = entreprise.Id;
                    int currentTickerId = 0;
                    var existingTicker = await _historicalTickerRepository.GetExistingTicker(entrepriseId, data.Date);
                    if (existingTicker != null)
                    {
                        currentTickerId = existingTicker.Id;
                    }
                    else
                    {
                        var newTicker = new HistoricalTicker
                        {
                            EntrepriseId = entrepriseId,
                            High = data.High,
                            Close = data.Close,
                            Low = data.Low,
                            Open = data.Open,
                            ReferenceDate = data.Date,
                            Volume = data.Volume
                        };

                        await _historicalTickerRepository.Insert(newTicker);
                        currentTickerId = newTicker.Id;
                    }

                    var newItemLog = new RequestLogTicker
                    {
                        TickerId = currentTickerId,
                        RequestLogId = newLog.Id
                    };

                    await _requestLogTickerRepository.Insert(newItemLog);
                }

                return new ResultModel { Data = stockData.Data };
            }
            else
            {
                var newLog = new RequestLog
                {
                    RequestJson = JsonConvert.SerializeObject(model),
                    ResponseJson = historicalData.Errors as string,
                    Status = "Fail",
                    UserId = userId
                };

                await _requestLogRepository.Insert(newLog);
                return new ResultModel { Errors = historicalData.Errors as string };
            }
        }
    }
}
