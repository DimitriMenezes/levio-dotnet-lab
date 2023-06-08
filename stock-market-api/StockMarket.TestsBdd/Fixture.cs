using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Concrete;
using StockMarket.Domain.Context;
using System;
using FluentValidation;
using Microsoft.Extensions.Configuration;
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
using StockMarket.Service.Abstract;
using Moq;

namespace StockMarket.TestsBdd
{
    public class Fixture : IDisposable
    {
        private StockMarketContext _dbContext;
        public UserRepository UserRepository;
        public EntrepriseRepository EntrepriseRepository;
        public SecurityService SecurityService;
        public RealTimeTickerRepository RealTimeTickerRepository;
        public RequestLogRepository RequestLogRepository;
        public RequestLogTickerRepository RequestLogTickerRepository;
        public HistoricalTickerRepository HistoricalTickerRepository;
        public TickerService TickerService;
        IConfiguration _configuration;
        public IMapper Mapper;

        public Fixture()
        {
            var options = new DbContextOptionsBuilder<StockMarketContext>()
            .UseInMemoryDatabase("TesteBddDb").Options;
            _dbContext = new StockMarketContext(options);
            UserRepository = new UserRepository(_dbContext);
            EntrepriseRepository = new EntrepriseRepository(_dbContext);
            RealTimeTickerRepository = new RealTimeTickerRepository(_dbContext);
            RequestLogRepository = new RequestLogRepository(_dbContext);
            HistoricalTickerRepository = new HistoricalTickerRepository(_dbContext);
            RequestLogTickerRepository = new RequestLogTickerRepository(_dbContext);
            var externalApi = new Mock<IExternalStockMarketApiService>();
            
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
            TickerService = new TickerService(EntrepriseRepository, RequestLogRepository, HistoricalTickerRepository,
                RequestLogTickerRepository, RealTimeTickerRepository, externalApi.Object, Mapper);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
