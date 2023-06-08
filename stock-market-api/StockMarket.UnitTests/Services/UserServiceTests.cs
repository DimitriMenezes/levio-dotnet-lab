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
    public class UserServiceTests : IClassFixture<Fixture>
    {
        Fixture _fixture;
        Mock<IValidator<UserModel>> validator;

        public UserServiceTests(Fixture fixture)
        {
            _fixture = fixture;
            validator = new Mock<IValidator<UserModel>>();
        }

        [Fact]
        public async Task CreateUser_ShouldSaveSucessfully()
        {
            //Arrange
            validator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var service = new UserService(_fixture.UnitOfWork, _fixture.SecurityService, _fixture.Mapper, validator.Object);
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
            validator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
              .ReturnsAsync(new FluentValidation.Results.ValidationResult
              {
                  Errors = new List<FluentValidation.Results.ValidationFailure>
                  {
                      new FluentValidation.Results.ValidationFailure { ErrorMessage = "Error" }
                  }
              });

            var service = new UserService(_fixture.UnitOfWork, _fixture.SecurityService, _fixture.Mapper, validator.Object);

            //Act
            var result = await service.CreateUser(new UserModel { Email = "wrong)email", Name = "test", Password = "1234556" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True(result.Errors != null);
        }

        [Fact]
        public async Task CreateUser_ShouldGiveEmailAlreadyRegistredError()
        {
            //Arrange
            validator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            await _fixture.UnitOfWork.UserRepository.Insert(new User { Email = "already_registred@testeee.com", Name = "Test", Password = "asd" });
            await _fixture.UnitOfWork.SaveChanges();

            var service = new UserService(_fixture.UnitOfWork, _fixture.SecurityService, _fixture.Mapper, validator.Object);

            //Act
            var result = await service.CreateUser(new UserModel { Email = "already_registred@testeee.com", Name = "Autre test", Password = "sgdfs" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True(result.Errors != null);
            Assert.True((string) result.Errors == "Email already registred");
        }
    }
}
