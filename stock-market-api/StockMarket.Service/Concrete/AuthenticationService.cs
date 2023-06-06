using AutoMapper;
using FluentValidation;
using StockMarket.Data.Abstract;
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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private IValidator<LoginModel> _validator;

        public AuthenticationService(IUserRepository userRepository, ISecurityService securityService, IMapper mapper, IValidator<LoginModel> validator)
        {
            _userRepository = userRepository;
            _securityService = securityService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ResultModel> Login(LoginModel model)
        {
            var validator = await _validator.ValidateAsync(model);
            if(!validator.IsValid)
                return new ResultModel { Errors = validator.Errors };

            var client = await _userRepository.GetByEmail(model.Email);
            if (client == null)
                return new ResultModel { Errors = "Email or password is incorrect" };

            var parts = client.Password.Split('.', 3);

            if (!_securityService.ValidatePassword(parts[1], parts[2], model.Password))
                return new ResultModel { Errors = "Email or password is incorrect" };

            var token = _securityService.GenerateToken(client);

            return new ResultModel
            {
                Data = new
                {
                    Token = token,
                    Client = _mapper.Map<UserModel>(client)
                }
            };
        }
    }
}
