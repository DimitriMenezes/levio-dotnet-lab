using AutoMapper;
using Newtonsoft.Json;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using StockMarket.Service.Validator;
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

        public async Task<ResultModel> DeleteEntreprise(int id)
        {
            try
            {
                await _entrepriseRepository.Delete(id);
                return new ResultModel { Data = "" };
            }
            catch (Exception ex)
            {
                return new ResultModel { Errors = ex.Message };
            }
          
        }

        public async Task<ResultModel> GetEntrepriseById(int id)
        {
            return new ResultModel { Data = await _entrepriseRepository.GetById(id) };
        }

        public async Task<ResultModel> SaveEntreprise(EntrepriseModel model)
        {
            var validator = new EntrepriseValidator().Validate(model);
            if(!validator.IsValid)
            {
                return new ResultModel { Errors = validator.Errors };
            }

            if (await _entrepriseRepository.GetByCode(model.Code) != null)
            {
                return new ResultModel { Errors = "Entreprise Code already registred." };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _entrepriseRepository.Insert(entity);
            
            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }

        public async Task<ResultModel> UpdateEntreprise(EntrepriseModel model)
        {
            var validator = new EntrepriseValidator().Validate(model);
            if (!validator.IsValid)
            {
                return new ResultModel { Errors = validator.Errors };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _entrepriseRepository.Update(entity);

            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }
    }
}
