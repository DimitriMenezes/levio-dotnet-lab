using FluentValidation;
using Moq;
using StockMarket.Service.Concrete;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace StockMarket.UnitTests.Services
{
    public class AuthenticationServiceTests : IClassFixture<Fixture>
    {
        Fixture _fixture;
        Mock<IValidator<LoginModel>> validator;
        Mock<IValidator<UserModel>> _userValidator;

        public AuthenticationServiceTests(Fixture fixture)
        {
            _fixture = fixture;
            validator = new Mock<IValidator<LoginModel>>();
            _userValidator = new Mock<IValidator<UserModel>>();

            
        }

        [Fact]
        public async Task Login_ShouldReturnToken()
        {
            //Arrange
            var userService = new UserService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, _userValidator.Object);
            _userValidator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var userCreation = await userService.CreateUser(new UserModel { Email = "login@success.com", Name = "test", Password = "1234556" });


            validator.Setup(m => m.ValidateAsync(It.IsAny<LoginModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var service = new AuthenticationService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, validator.Object);
            //Act
            var result = await service.Login(new LoginModel { Email = "login@success.com",  Password = "1234556" });
            //Assert
            Assert.True(result.Data != null);
            Assert.True(result.Errors == null);
        }

        [Fact]
        public async Task LoginWrongEmailValidation_ShouldReturnError()
        {
            //Arrange         
            validator.Setup(m => m.ValidateAsync(It.IsAny<LoginModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { Errors = new List<FluentValidation.Results.ValidationFailure> 
                { new FluentValidation.Results.ValidationFailure { ErrorMessage = "Error" } } });
            var service = new AuthenticationService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, validator.Object);
            //Act
            var result = await service.Login(new LoginModel { Email = "wrong_email", Password = "1234556" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True(result.Errors != null);
        }

        [Fact]
        public async Task LoginWrongPassword_ShouldReturnError()
        {
            //Arrange
            var userService = new UserService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, _userValidator.Object);
            _userValidator.Setup(m => m.ValidateAsync(It.IsAny<UserModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var userCreation = await userService.CreateUser(new UserModel { Email = "login@wrongpassword.com", Name = "test", Password = "1234556" });

            validator.Setup(m => m.ValidateAsync(It.IsAny<LoginModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var service = new AuthenticationService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, validator.Object);
            //Act
            var result = await service.Login(new LoginModel { Email = "login@wrongpassword.com", Password = "1234556789" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True((string) result.Errors == "Email or password is incorrect");
        }

        [Fact]
        public async Task LoginUserNotFound_ShouldReturnError()
        {
            //Arrange
            validator.Setup(m => m.ValidateAsync(It.IsAny<LoginModel>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });
            var service = new AuthenticationService(_fixture.UserRepository, _fixture.SecurityService, _fixture.Mapper, validator.Object);
            //Act
            var result = await service.Login(new LoginModel { Email = "user_not_found@email.com", Password = "1234556789" });
            //Assert
            Assert.True(result.Data == null);
            Assert.True(result.Errors != null);
        }

    }
}
