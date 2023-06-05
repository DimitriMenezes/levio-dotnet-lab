using AutoMapper;
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
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        public UserService(ISecurityService securityService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
            _securityService = securityService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResultModel> CreateUser(UserModel model)
        {
            var clientValidator = new UserValidator().Validate(model);
            if (!clientValidator.IsValid)
                return new ResultModel { Errors = clientValidator.Errors };

            var existingEntity = (await _userRepository.GetAll()).FirstOrDefault(i => i.Email == model.Email);
            if (existingEntity != null)
                return new ResultModel { Errors = "Email already registred" };

            model.Password = _securityService.EncodePassword(model.Password);
            var newEntity = _mapper.Map<User>(model);
            try
            {
                await _userRepository.Insert(newEntity);
                _unitOfWork.Commit();
                return new ResultModel { Data = _mapper.Map<UserModel>(newEntity) };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResultModel { Errors = ex.Message };
            }
        }

        public async Task<ResultModel> GetUserById(int id)
        {
            return new ResultModel { Data = _mapper.Map<UserModel>(await _userRepository.GetById(id)) };
        }

        public async Task<ResultModel> UpdateUser(UserModel model)
        {
            var validator = new UserValidator().Validate(model);
            if (!validator.IsValid)
                return new ResultModel { Errors = validator.Errors };

            var user = await _userRepository.GetById(model.Id);
            if (user == null)
                return new ResultModel { Errors = "User not Found" };

            user.Password = _securityService.EncodePassword(model.Password);
            try
            {
                await _userRepository.Update(user);
                _unitOfWork.Commit();
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
