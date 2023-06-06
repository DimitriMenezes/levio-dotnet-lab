using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StockMarket.Service.Abstract;
using StockMarket.Service.Concrete;
using StockMarket.Service.Model;
using StockMarket.Service.Validator;

namespace StockMarket.Service.IoC
{
    public static class ServicesInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);            
            services.AddScoped<IUserService, UserService>();            
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IEntrepriseService, EntrepriseService>();
            services.AddScoped<ITickerService, TickerService>();
            services.AddScoped<IExternalStockMarketApiService, ExternalStockMarketApiService>();

            services.AddScoped<IValidator<LoginModel>, LoginValidator>();
            services.AddScoped<IValidator<EntrepriseModel>, EntrepriseValidator>();
            services.AddScoped<IValidator<UserModel>, UserValidator>();
        }
    }
}
