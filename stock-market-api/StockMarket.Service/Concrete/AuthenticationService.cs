using AutoMapper;
using StockMarket.Data.Abstract;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
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
        private readonly IMapper _mapper;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;            
            _mapper = mapper;
        }

        public async Task<ResultModel> Login(LoginModel model)
        {
            var client = await _userRepository.GetByEmail(model.Email);
            if (client == null)
                return new ResultModel { Errors = "Email or password is incorrect" };

            var parts = client.Password.Split('.', 3);

            if (!PasswordService.IsPasswordCorrect(parts[1], parts[2], model.Password))
                return new ResultModel { Errors = "Email or password is incorrect" };

            var token = TokenService.GenerateToken(client);

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
