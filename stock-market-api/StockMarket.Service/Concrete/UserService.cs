using AutoMapper;
using FluentValidation;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using StockMarket.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserModel> _validator;
        public UserService(IUnitOfWork unitOfWork, ISecurityService securityService, IMapper mapper, IValidator<UserModel> validator)
        {
            _unitOfWork = unitOfWork;
            _securityService = securityService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ResultModel> CreateUser(UserModel model)
        {
            try
            {
                var clientValidator = await _validator.ValidateAsync(model);
                if (!clientValidator.IsValid)
                    return new ResultModel { Errors = clientValidator.Errors };

                var existingEntity = await _unitOfWork.UserRepository.GetByEmail(model.Email);
                if (existingEntity != null)
                    return new ResultModel { Errors = "Email already registred" };

                model.Password = _securityService.EncodePassword(model.Password);
                var newEntity = _mapper.Map<User>(model);
                await _unitOfWork.UserRepository.Insert(newEntity);
                await _unitOfWork.SaveChanges();
                return new ResultModel { Data = _mapper.Map<UserModel>(newEntity) };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResultModel { Errors = ex.Message};
            }
           
        }

        public async Task<ResultModel> GetUserById(int id)
        {
            return new ResultModel { Data = _mapper.Map<UserModel>(await _unitOfWork.UserRepository.GetById(id)) };
        }

        public async Task<ResultModel> UpdateUser(UserModel model)
        {
            try
            {
                var validator = await _validator.ValidateAsync(model);
                if (!validator.IsValid)
                    return new ResultModel { Errors = validator.Errors };

                var user = await _unitOfWork.UserRepository.GetById(model.Id);
                if (user == null)
                    return new ResultModel { Errors = "User not Found" };

                user.Password = _securityService.EncodePassword(model.Password);
                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChanges();

                return new ResultModel { Data = _mapper.Map<UserModel>(user) };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResultModel { Errors = ex.Message };
            }
          
        }
    }
}
