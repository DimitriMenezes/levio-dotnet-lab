using StockMarket.Data.Concrete;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Concrete;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace StockMarket.TestsBdd.StepDefinitions
{
    [Binding]
    public class StockMarketSearchStepDefinitions : IClassFixture<Fixture>
    {
        private Fixture _fixture;        
        private int _userId;
        public StockMarketSearchStepDefinitions(Fixture fixture)
        {
            _fixture = fixture;            
        }

        [Given(@"I am registred")]
        public async Task GivenIAmRegistred(Table table)
        {
            var user = table.CreateInstance<User>();
            await _fixture.UnitOfWork.UserRepository.Insert(user);
            await _fixture.UnitOfWork.SaveChanges();
            Assert.True(user.Id != 0);
            _userId = user.Id;
        }

        [Given(@"I Have Entreprises registred")]
        public async Task GivenIHaveEntreprisesRegistred(Table table)
        {
            var entreprises = table.CreateSet<Entreprise>();
            foreach (var item in entreprises)
            {
                await _fixture.UnitOfWork.EntrepriseRepository.Insert(item);
                await _fixture.UnitOfWork.SaveChanges();
                Assert.True(item.Id != 0);
            }            
        }

        [Given(@"I Have Real Time Tickers registred")]
        public async Task GivenIHaveRealTimeTickersRegistred(Table table)
        {
            var entreprises = await _fixture.UnitOfWork.EntrepriseRepository.GetAll();
            foreach (var row in table.Rows)
            {
                var ticker = new RealTimeTicker
                {
                    ReferenceDate = DateTime.Parse(row["date"]),
                    EntrepriseId = entreprises.FirstOrDefault(i => i.Code == row["code"]).Id,
                    Current = Convert.ToDecimal(row["current"]),
                    Low = Convert.ToDecimal(row["low"]),
                    High = Convert.ToDecimal(row["high"])
                };
                await _fixture.UnitOfWork.RealTimeTickerRepository.Insert(ticker);
                await _fixture.UnitOfWork.SaveChanges();
                Assert.True(ticker.Id != 0);
            }                
        }

        [When(@"I search for Real Time Tickers")]
        public void WhenISearchForRealTimeTickers(Table table)
        {
            var codes = table.CreateSet<string>();
            var result = _fixture.TickerService.GetRealTimeData(new Service.Model.Filter.TickerFilterModel { Entreprises = codes.ToList() }, _userId);
        }


        [When(@"I search for Historical Tickers")]
        public void WhenISearchForHistoricalTickers(Table table)
        {
            
        }

        [Then(@"the result is")]
        public void ThenTheResultIs(Table table)
        {
            
        }

        [Then(@"A search log is saved")]
        public void ThenASearchLogIsSaved()
        {
            
        }

        [Given(@"I Have Historical Tickers registred")]
        public void GivenIHaveHistoricalTickersRegistred(Table table)
        {
            
        }
    }
}
