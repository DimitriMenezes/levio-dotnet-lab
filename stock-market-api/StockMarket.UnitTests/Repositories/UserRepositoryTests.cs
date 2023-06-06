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
    public class UserRepositoryTests
    {
        public Mock<StockMarketContext> dbMock = new Mock<StockMarketContext>();
        public Mock<DbSet<User>> dbSetUser = new Mock<DbSet<User>>();

        [Fact]
        public async Task GetByEmail_Should_Return_Value()
        {
            //Arrange
            dbSetUser.Object.Add(new User { Email = "test@test.com" });
            var teste = await dbSetUser.Object.ToListAsync();
            dbMock.Setup(i => i.User).Returns(dbSetUser.Object);
           
            var repository = new UserRepository(dbMock.Object);
           
            //Act
            var result = await repository.GetByEmail("test@test.com");
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByEmail_Should_Return_Empty()
        {
            //Arrange            
            dbMock.Setup(i => i.User).Returns(dbSetUser.Object);
            var repository = new UserRepository(dbMock.Object);
            dbMock.Setup(i => i.Add(new User { Email = "test@test.com" }));
            dbMock.Setup(i => i.SaveChanges());
            //Act
            var result = await repository.GetByEmail("wrong_test@test.com");
            //Assert
            Assert.Null(result);
        }
    }
}
