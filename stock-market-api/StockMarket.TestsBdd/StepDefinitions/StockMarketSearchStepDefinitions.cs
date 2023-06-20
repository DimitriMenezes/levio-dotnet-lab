using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using StockMarket.Data.Concrete;
using StockMarket.Domain.Entities;
using StockMarket.Service.Abstract;
using StockMarket.Service.Concrete;
using StockMarket.Service.Model;
using StockMarket.Service.Model.Filter;
using System;
using System.Security.Claims;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace StockMarket.TestsBdd.StepDefinitions
{
    [Binding]
    public class StockMarketSearchStepDefinitions //: IClassFixture<Fixture>
    {
        //private Fixture _fixture;
        private int _userId;
        private OkObjectResult endpointResult;

        public WebApplicationFactory<Program> _factory { get; }
        public HttpClient _httpClient;
        public StockMarketSearchStepDefinitions(WebApplicationFactory<Program> Factory)
        {
            _factory = Factory;
            _httpClient = Factory.CreateDefaultClient(new Uri("http://localhost/"));
        }

        //public StockMarketSearchStepDefinitions(Fixture fixture)
        //{
        //    _fixture = fixture;
        //}

        [Given(@"I am registred")]
        public async Task GivenIAmRegistred(Table table)
        {
            var testando = await _httpClient.GetAsync("User/1");

            /*
            var userModel = table.CreateInstance<UserModel>();

            var result = await _fixture.UserController.Post(userModel);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var newUser = ((okResult.Value as ResultModel).Data as UserModel);
            Assert.NotNull(newUser);
            _userId = newUser.Id;
            */
        }

        [Given(@"I Have Entreprises registred")]
        public async Task GivenIHaveEntreprisesRegistred(Table table)
        {
            /*
            var entreprises = table.CreateSet<EntrepriseModel>();
            foreach (var item in entreprises)
            {
                var result = await _fixture.EntrepriseController.Post(item);
                Assert.IsType<OkObjectResult>(result);
            }        
            */
        }

        [When(@"I search for Real Time Tickers")]
        public async Task WhenISearchForRealTimeTickers(Table table)
        {
            /*
            var codes = table.CreateSet<string>();
            var result = await _fixture.TickerController
                .GetRealTimeData(new RealTimeTickerFilterModel 
                { 
                    EntrepriseCodes = codes.ToList()
                });
            Assert.IsType<OkObjectResult>(result);
            endpointResult = result as OkObjectResult;
            */
        }


        [When(@"I search for Historical Tickers")]
        public async Task WhenISearchForHistoricalTickers(Table table)
        {
            /*
            var claims = new System.Security.Claims.ClaimsPrincipal
            {
                
                
            };
            _fixture.TickerController.HttpContext.User.AddIdentity(
                 new ClaimsIdentity(new List<Claim>
                {
                    new Claim("Id", _userId.ToString())
                }));

            var codes = table.CreateInstance<HistoricTickerFilterModel>();
            //_fixture.TickerController.HttpContext.User = new System.Security.Claims.ClaimsPrincipal { Claims = new  }
            var result = await _fixture.TickerController
                .GetHistoricalData(codes);
            Assert.IsType<OkObjectResult>(result);
            endpointResult = result as OkObjectResult;
            */
        }

        [Then(@"the result is")]
        public void ThenTheResultIs(Table table)
        {
            
        }

        [Then(@"A search log is saved")]
        public void ThenASearchLogIsSaved()
        {
            
        }
    }
}
