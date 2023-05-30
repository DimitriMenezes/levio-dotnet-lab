﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StockMarket.Service.Abstract;
using StockMarket.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddScoped<IEntrepriseService, EntrepriseService>();
            services.AddScoped<ITickerService, TickerService>();
        }
    }
}