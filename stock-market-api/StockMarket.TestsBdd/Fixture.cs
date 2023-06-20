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
using StockMarket.Api.Controllers;
using StockMarket.Service.Validator;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace StockMarket.TestsBdd
{
    public class Fixture : IDisposable
    {
        private StockMarketContext _dbContext;
        private IUnitOfWork UnitOfWork { get; set; }
        private IConfiguration _configuration;
        private IMapper Mapper;
        public WebApplicationFactory<Program> Factory { get; }

        private SecurityService SecurityService;
        private TickerService TickerService;
        private AuthenticationService AuthenticationService;
        private UserService UserService;
        private EntrepriseService EntrepriseService;

        public AuthenticationController AuthenticationController;
        public UserController UserController;
        public EntrepriseController EntrepriseController;
        public TickerController TickerController;
        public HttpClient HttpClient;

        public Fixture()
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri("http://localhost/");

            var options = new DbContextOptionsBuilder<StockMarketContext>()
            .UseInMemoryDatabase("TesteBddDb").Options;
            _dbContext = new StockMarketContext(options);

            UnitOfWork = new UnitOfWork(_dbContext);
            var externalApi = new Mock<IExternalStockMarketApiService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            Mapper = mappingConfig.CreateMapper();

            var SecuritySettings = @"{
            ""KeyVaultUrl"" : ""https://stockmarket.vault.azure.net/"",
            ""StockDataApiUrl"" : ""https://api.stockdata.org/v1/data/""           
            }";

            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(SecuritySettings)));

            _configuration = builder.Build();


            SecurityService = new SecurityService(_configuration);
            TickerService = new TickerService(externalApi.Object, UnitOfWork, Mapper);
            AuthenticationService = new AuthenticationService(UnitOfWork.UserRepository, SecurityService, Mapper, new LoginValidator());
            UserService = new UserService(UnitOfWork, SecurityService, Mapper, new UserValidator());
            EntrepriseService = new EntrepriseService(UnitOfWork, Mapper, new EntrepriseValidator());


            AuthenticationController = new AuthenticationController(AuthenticationService);
            UserController = new UserController(UserService);
            EntrepriseController = new EntrepriseController(EntrepriseService, null);
            TickerController = new TickerController(TickerService);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
