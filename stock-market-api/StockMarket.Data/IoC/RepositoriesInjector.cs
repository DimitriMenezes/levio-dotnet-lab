using Microsoft.Extensions.DependencyInjection;
using StockMarket.Data.Abstract;
using StockMarket.Data.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.IoC
{
    public static class RepositoriesInjector
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IRequestLogRepository, RequestLogRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEntrepriseRepository, EntrepriseRepository>();
            services.AddScoped<IHistoricalTickerRepository, HistoricalTickerRepository>();
            services.AddScoped<IRealTimeTickerRepository, RealTimeTickerRepository>();
            services.AddScoped<IRequestLogTickerRepository, RequestLogTickerRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
