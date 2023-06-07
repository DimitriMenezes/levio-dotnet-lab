using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StockMarket.UnitTests.Repositories
{
    public class EntrepriseRepositoryTests : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;
        public EntrepriseRepositoryTests(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetByCode_Should_Return_Value()
        {
            //Arrange
            await _fixture.EntrepriseRepository.Insert(new Domain.Entities.Entreprise { Code = "ABC", Name = "Entreprise1" });
            //Act
            var result = await _fixture.EntrepriseRepository.GetByCode("ABC");
            //Assert
            Assert.True(result != null);
        }

        [Fact]
        public async Task GetByCode_Should_Return_Null()
        {
            //Act
            var result = await _fixture.EntrepriseRepository.GetByCode("ABCD");
            //Assert
            Assert.True(result == null);
        }

        [Fact]
        public async Task GetByCodes_Should_Return_Value()
        {
            //Arrange
            await _fixture.EntrepriseRepository.Insert(new Domain.Entities.Entreprise { Code = "ABC1", Name = "Entreprise1" });
            await _fixture.EntrepriseRepository.Insert(new Domain.Entities.Entreprise { Code = "ABC2", Name = "Entreprise2" });
            //Act
            var result = await _fixture.EntrepriseRepository.GetByCodeList(new List<string> { "ABC1", "ABC2" });
            //Assert
            Assert.True(result != null);
        }

        [Fact]
        public async Task GetByCodes_Should_Return_Null()
        {
            var result = await _fixture.EntrepriseRepository.GetByCodeList(new List<string> { "ABCASD1", "ASDASD" });
            //Assert
            Assert.True(!result.Any());
        }
    }
}
