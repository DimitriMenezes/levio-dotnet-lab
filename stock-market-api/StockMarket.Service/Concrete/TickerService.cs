using AutoMapper;
using Newtonsoft.Json;
using StockMarket.Data.Abstract;
using StockMarket.Data.Concrete;
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
        private readonly IExternalStockMarketApiService _externalApiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TickerService(            
            IExternalStockMarketApiService externalApiService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {           
            _externalApiService = externalApiService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ResultModel> GetRealTimeData(RealTimeTickerFilterModel model, int userId)
        {
            var entreprises = await _unitOfWork.EntrepriseRepository.GetByCodeList(model.EntrepriseCodes);
            var realTimeData = await _externalApiService.GetRealTimeData(model.EntrepriseCodes);
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

                await _unitOfWork.RequestLogRepository.Insert(newLog);                

                foreach (var data in stockData.Data)
                {
                    int currentTickerId = 0;
                    var newTicker = _mapper.Map<RealTimeTicker>(data);
                    newTicker.EntrepriseId = entreprises.FirstOrDefault(i => i.Code == data.Ticker).Id;

                    await _unitOfWork.RealTimeTickerRepository.Insert(newTicker);
                    currentTickerId = newTicker.Id;
                    
                    var newItemLog = new RequestLogTicker
                    {                        
                        RequestLog = newLog,
                        Ticker = newTicker
                    };

                    await _unitOfWork.RequestLogTickerRepository.Insert(newItemLog);
                }
                await _unitOfWork.SaveChanges();
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

                await _unitOfWork.RequestLogRepository.Insert(newLog);
                await _unitOfWork.SaveChanges();
                return new ResultModel { Errors = realTimeData.Errors as string };
            }
        }

        public async Task<ResultModel> GetHistoricalData(HistoricTickerFilterModel model, int userId)
        {
            var entreprise = await _unitOfWork.EntrepriseRepository.GetByCode(model.EntrepriseCode);
            var historicalData = await _externalApiService.GetHistoricalData(model.EntrepriseCode, model.Start, model.End);
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

                await _unitOfWork.RequestLogRepository.Insert(newLog);

                foreach (var data in stockData.Data)
                {
                    var entrepriseId = entreprise.Id;
                    HistoricalTicker currentTicker;
                    var existingTicker = await _unitOfWork.HistoricalTickerRepository.GetExistingTicker(entrepriseId, data.Date);
                    if (existingTicker != null)
                    {
                        currentTicker = existingTicker;
                    }
                    else
                    {
                        var newTicker = _mapper.Map<HistoricalTicker>(data);
                        newTicker.EntrepriseId = entrepriseId;

                        await _unitOfWork.HistoricalTickerRepository.Insert(newTicker);
                        currentTicker = newTicker;
                    }

                    var newItemLog = new RequestLogTicker
                    {
                        Ticker = currentTicker,
                        RequestLog = newLog
                    };

                    await _unitOfWork.RequestLogTickerRepository.Insert(newItemLog);
                    await _unitOfWork.SaveChanges();
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

                await _unitOfWork.RequestLogRepository.Insert(newLog);
                await _unitOfWork.SaveChanges();
                return new ResultModel { Errors = historicalData.Errors as string };
            }
        }
    }
}
