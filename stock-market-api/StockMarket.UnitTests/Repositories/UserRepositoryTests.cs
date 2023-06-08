using Microsoft.EntityFrameworkCore;
using Moq;
using StockMarket.Data.Abstract;
using StockMarket.Data.Concrete;
using StockMarket.Domain.Context;
using StockMarket.Domain.Entities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StockMarket.UnitTests
{
    public class UserRepositoryTests : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public UserRepositoryTests(Fixture fixture) 
        {
            _fixture = fixture;
        }    

        [Fact]
        public async Task GetByEmail_Should_Return_Value()
        {
            //Arrange            
            await _fixture.UnitOfWork.UserRepository.Insert(new User { Email = "teste@testeee.com", Name = "Test", Password="asd" });
            await _fixture.UnitOfWork.SaveChanges();
            //Act
            var result = await _fixture.UnitOfWork.UserRepository.GetByEmail("teste@testeee.com");
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByEmail_Should_Return_Empty()
        {
            //Arrange            
           
            //Act
            var result = await _fixture.UnitOfWork.UserRepository.GetByEmail("wrong_test@test.com");
            //Assert
            Assert.Null(result);
        }
    }
}
