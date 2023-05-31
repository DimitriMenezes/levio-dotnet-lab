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
        private readonly IRequestLogTickerRepository _requestLogTickerRepository;
        private readonly IExternalStockMarketApiService _externalApiService;
        private readonly IMapper _mapper;

        public TickerService(
            IEntrepriseRepository entrepriseRepository,
            IRequestLogRepository requestLogRepository,
            IHistoricalTickerRepository historicalTickerRepository,
            IRequestLogTickerRepository requestLogTickerRepository,
            IExternalStockMarketApiService externalApiService,
            IMapper mapper)
        {
            _entrepriseRepository = entrepriseRepository;            
            _requestLogRepository = requestLogRepository;
            _historicalTickerRepository = historicalTickerRepository;
            _requestLogTickerRepository = requestLogTickerRepository;
            _externalApiService = externalApiService;
            _mapper = mapper;
        }


        public async Task GetRealTimeData(string code)
        {

        }

        public async Task<ResultModel> GetHistoricalData(TickerFilterModel model)
        {
            var entreprise = await _entrepriseRepository.GetById(model.EntrepriseId);
            var historicalData = await _externalApiService.GetHistoricalData(entreprise.Code, model.Start, model.End);
            if (historicalData.Data != null)
            {                
                var stockData = historicalData.Data as StockDataApiResultModel;
                var newLog = new RequestLog
                {
                    RequestJson = JsonConvert.SerializeObject(model),
                    ResponseJson = JsonConvert.SerializeObject(historicalData.Data),
                    Status = "Success",
                    UserId = model.UserId
                };
                
                await _requestLogRepository.Insert(newLog);

                foreach(var data in stockData.Data)
                {
                    int currentTickerId = 0;
                    var existingTicker = await _historicalTickerRepository.GetExistingTicker(model.EntrepriseId, data.Date);
                    if(existingTicker != null)
                    {
                        currentTickerId = existingTicker.Id;
                    }
                    else
                    {
                        var newTicker = new HistoricalTicker
                        {
                            EntrepriseId = model.EntrepriseId,
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
                    UserId = model.UserId
                };

                await _requestLogRepository.Insert(newLog);
                return new ResultModel { Errors = historicalData.Errors as string };
            }
        }
    }
}
