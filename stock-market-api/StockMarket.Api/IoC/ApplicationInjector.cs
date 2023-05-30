using Microsoft.Extensions.DependencyInjection;
using StockMarket.Data.IoC;
using StockMarket.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMarket.Api.IoC
{
    public static class ApplicationInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            RepositoriesInjector.RegisterRepositories(services);
            ServicesInjector.RegisterServices(services);
        }
    }
}
