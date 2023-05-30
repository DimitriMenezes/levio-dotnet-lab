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
        private readonly IMapper _mapper;
        public EntrepriseService(IEntrepriseRepository entrepriseRepository, IMapper mapper)
        {
            _entrepriseRepository = entrepriseRepository;            
            _mapper = mapper;
        }

        public async Task<ResultModel> SaveEntreprise(EntrepriseModel model)
        {
            if(await _entrepriseRepository.GetByCode(model.Code) != null)
            {
                return new ResultModel { Errors = "Entreprise Code already registred." };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _entrepriseRepository.Insert(entity);
            
            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }
    }
}
