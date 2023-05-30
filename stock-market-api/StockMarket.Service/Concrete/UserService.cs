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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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

            model.Password = PasswordService.EncodePassword(model.Password);
            var newEntity = _mapper.Map<User>(model);
            await _userRepository.Insert(newEntity);
            return new ResultModel { Data = _mapper.Map<UserModel>(newEntity) };
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

            user.Password = PasswordService.EncodePassword(model.Password);
            await _userRepository.Update(user);

            return new ResultModel { Data = _mapper.Map<UserModel>(user) };
        }
    }
}
