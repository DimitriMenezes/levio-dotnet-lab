using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StockMarket.Data.Abstract;
using StockMarket.Data.Concrete;
using StockMarket.Domain.Context;
using StockMarket.Domain.Entities;
using StockMarket.Service.Concrete;
using StockMarket.Service.IoC;
using StockMarket.Service.Model;
using StockMarket.Service.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StockMarket.UnitTests.Services
{
    public class UserServiceTests
    {
        StockMarketContext dbContext;
        UserRepository userRepository;        
        IConfiguration configuration;        
        IMapper mapper;
        Mock<IValidator<UserModel>> validator;

        public UserServiceTests()
        {            
            var options = new DbContextOptionsBuilder<StockMarketContext>()
                   .UseInMemoryDatabase("TesteDb").Options;
            dbContext = new StockMarketContext(options);          
            userRepository = new UserRepository(dbContext);

            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            mapper = mappingConfig.CreateMapper();

            validator = new Mock<IValidator<UserModel>>();

            var SecuritySettings = @"{""SecuritySettings"":{
            ""HashSecret"" : ""sadfasfsadfsd345325432"",
            ""KeySize"" : ""8"",
            ""SaltSize"" : ""8"",
            ""Iteration"" : ""8""
            }}";

            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(SecuritySettings)));

            var configuration1 = builder.Build();
            configuration = configuration1;
        }

        [Fact]
        public async Task CreateUser_ShouldSaveSucessfully()
        {
            //Arrange          
            var securityService = new SecurityService(configuration);

            validator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult {});
                        
            var service = new UserService(userRepository, securityService, mapper, validator.Object);
            //Act
            var result = await service.CreateUser(new UserModel { Email = "test@test.com", Name = "test", Password = "1234556" });
            //Assert
            Assert.True(result.Data != null);
            Assert.True(result.Errors == null);
        }

        [Fact]
        public async Task CreateUser_ShouldGiveValidationError()
        {
            //Arrange
            var securityService = new SecurityService(configuration);
            validator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
              .ReturnsAsync(new FluentValidation.Results.ValidationResult { 
                  Errors = new List<FluentValidation.Results.ValidationFailure> 
                  { 
                      new FluentValidation.Results.ValidationFailure { ErrorMessage = "Error" } 
                  } 
              });

            var service = new UserService(userRepository, securityService, mapper, validator.Object);

            //Act
            var result = await service.CreateUser(new UserModel { Email = "wrong)email", Name = "test", Password = "1234556" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True(result.Errors != null);
        }
    }
}
