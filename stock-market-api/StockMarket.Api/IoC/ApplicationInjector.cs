using Microsoft.Extensions.DependencyInjection;
using StockMarket.Data.IoC;
using StockMarket.Service.IoC;
using StockMarket.Service.Settings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace StockMarket.Api.IoC
{
    public static class ApplicationInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RepositoriesInjector.RegisterRepositories(services);
            ServicesInjector.RegisterServices(services);

            var key = Encoding.ASCII.GetBytes(SecuritySettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
