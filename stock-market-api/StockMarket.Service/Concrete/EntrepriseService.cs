using AutoMapper;
using FluentValidation;
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
        private readonly IUnitOfWork _unitOfWork;               
        private readonly IMapper _mapper;
        private readonly IValidator<EntrepriseModel> _validator;
        public EntrepriseService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EntrepriseModel> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ResultModel> DeleteEntreprise(int id)
        {
            try
            {
                await _unitOfWork.EntrepriseRepository.Delete(id);
                await _unitOfWork.SaveChanges();
                return new ResultModel { Data = "" };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResultModel { Errors = ex.Message };
            }
          
        }

        public async Task<ResultModel> GetEntrepriseById(int id)
        {
            return new ResultModel { Data = await _unitOfWork.EntrepriseRepository.GetById(id) };
        }

        public async Task<ResultModel> SaveEntreprise(EntrepriseModel model)
        {
            var validator = await _validator.ValidateAsync(model);
            if(!validator.IsValid)
            {
                return new ResultModel { Errors = validator.Errors };
            }

            if (await _unitOfWork.EntrepriseRepository.GetByCode(model.Code) != null)
            {
                return new ResultModel { Errors = "Entreprise Code already registred." };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _unitOfWork.EntrepriseRepository.Insert(entity);
            await _unitOfWork.SaveChanges();
            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }

        public async Task<ResultModel> UpdateEntreprise(EntrepriseModel model)
        {
            var validator = await _validator.ValidateAsync(model);
            if (!validator.IsValid)
            {
                return new ResultModel { Errors = validator.Errors };
            }

            var entity = _mapper.Map<Entreprise>(model);
            var entityResult = await _unitOfWork.EntrepriseRepository.Update(entity);
            await _unitOfWork.SaveChanges();
            return new ResultModel { Data = _mapper.Map<EntrepriseModel>(entityResult) };
        }
    }
}
