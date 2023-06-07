using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Concrete;
using StockMarket.Domain.Context;
using System;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Moq;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using StockMarket.Service.Concrete;
using StockMarket.Service.IoC;
using StockMarket.Service.Model;
using StockMarket.Service.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StockMarket.UnitTests
{
    public class Fixture : IDisposable
    {
        private StockMarketContext _dbContext;
        public UserRepository UserRepository;
        public SecurityService SecurityService;
        IConfiguration _configuration;
        public IMapper Mapper;

        public Fixture()
        {
            var options = new DbContextOptionsBuilder<StockMarketContext>()
            .UseInMemoryDatabase("TesteDb").Options;
            _dbContext = new StockMarketContext(options);
            UserRepository = new UserRepository(_dbContext);


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            Mapper = mappingConfig.CreateMapper();

            var SecuritySettings = @"{""SecuritySettings"":{
            ""HashSecret"" : ""sadfasfsadfsd345325432"",
            ""KeySize"" : ""8"",
            ""SaltSize"" : ""8"",
            ""Iteration"" : ""8""
            }}";

            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(SecuritySettings)));

            _configuration = builder.Build();
            SecurityService = new SecurityService(_configuration);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
