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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntrepriseRepository _entrepriseRepository;
        private readonly IRequestLogRepository _requestLogRepository;
        private readonly IHistoricalTickerRepository _historicalTickerRepository;
        private readonly IRealTimeTickerRepository _realTimeTickerRepository;
        private readonly IRequestLogTickerRepository _requestLogTickerRepository;
        private readonly IExternalStockMarketApiService _externalApiService;
        private readonly IMapper _mapper;

        public TickerService(
            IExternalStockMarketApiService externalApiService,
            IUnitOfWork UnitOfWork,
            IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            _entrepriseRepository = _unitOfWork.EntrepriseRepository;
            _requestLogRepository = _unitOfWork.RequestLogRepository;
            _historicalTickerRepository = _unitOfWork.HistoricalTickerRepository;
            _realTimeTickerRepository = _unitOfWork.RealTimeTickerRepository;
            _requestLogTickerRepository = _unitOfWork.RequestLogTickerRepository;
            _externalApiService = externalApiService;
            _mapper = mapper;
        }


        public async Task<ResultModel> GetRealTimeData(TickerFilterModel model, int userId)
        {
            var result = new ResultModel();
            try
            {
                var entreprises = (await _entrepriseRepository.GetAll()).Where(i => model.EntrepriseId == i.Id);
                var realTimeData = await _externalApiService.GetRealTimeData(entreprises.Select(i => i.Code).ToList());
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

                    foreach (var data in stockData.Data)
                    {
                        int currentTickerId = 0;
                        var newTicker = new RealTimeTicker
                        {
                            EntrepriseId = model.EntrepriseId,
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

                    result.Data = stockData.Data;
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
                    result.Errors = realTimeData.Errors as string;
                }
                await _unitOfWork.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                result.Errors = ex.Message;
                return result;
            }
        }

        public async Task<ResultModel> GetHistoricalData(TickerFilterModel model, int userId)
        {
            var result = new ResultModel();
            try
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
                        UserId = userId
                    };

                    await _requestLogRepository.Insert(newLog);

                    foreach (var data in stockData.Data)
                    {
                        int currentTickerId = 0;
                        var existingTicker = await _historicalTickerRepository.GetExistingTicker(model.EntrepriseId, data.Date);
                        if (existingTicker != null)
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

                    result.Data = stockData.Data;
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

                    result.Errors = historicalData.Errors as string;
                }
                await _unitOfWork.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                result.Errors = ex.Message;
                return result;
            }
        }
    }
}
