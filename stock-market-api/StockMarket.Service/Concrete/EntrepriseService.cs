using AutoMapper;
using Newtonsoft.Json;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockMarket.Service.Concrete
{
    public class EntrepriseService : IEntrepriseService
    {
        private readonly IEntrepriseRepository _entrepriseRepository;
        private readonly ITickerRepository _tickerRepository;
        private readonly IMapper _mapper;
        public EntrepriseService(IEntrepriseRepository entrepriseRepository, ITickerRepository tickerRepository, IMapper mapper)
        {
            _entrepriseRepository = entrepriseRepository;
            _tickerRepository = tickerRepository;
            _mapper = mapper;
        }

        public async Task<ResultModel> SaveEntreprise(EntrepriseModel model)
        {
            if((await _entrepriseRepository.GetAll()).Any(i => i.Code == model.Code))
            {
                return new ResultModel { Errors = "Entreprise Code already registred." };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _entrepriseRepository.Insert(entity);

            /*
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.stockdata.org/v1/data/eod");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "QVzvCay9jQo5L35eIOtJGJdSw62DBS1WtsFnax5i");

            var result = await httpClient.GetAsync($"?symbols={model.Code}");
            if(result.IsSuccessStatusCode)
            {
                var stringContent = await result.Content.ReadAsStringAsync();
                var stockData = JsonConvert.DeserializeObject<StockDataApiResultModel>(stringContent);
                foreach(var data in stockData.Data)
                {
                    var newTicker = new Ticker
                    {
                        EntrepriseId = entityResult.Id,
                        Low = data.Low,
                        High = data.High,
                        Close = data.Close,
                        Open = data.Open,
                        Volume = data.Volume,
                        ReferenceDate = data.Date
                    };
                    await _tickerRepository.Insert(newTicker);
                }
            }
            else
            {
                //error and rollback
                return new ResultModel { ErrorMessages = result.ReasonPhrase };
            }

            */

            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }
    }
}
