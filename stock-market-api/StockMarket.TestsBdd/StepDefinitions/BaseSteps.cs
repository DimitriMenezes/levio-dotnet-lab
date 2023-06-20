using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.TestsBDD.StepDefinitions
{
    public class BaseSteps
    {
        public WebApplicationFactory<Program> _factory { get; }
        public HttpClient HttpClient;
        public BaseSteps(WebApplicationFactory<Program> Factory)
        {
            _factory = Factory;
            HttpClient = Factory.CreateDefaultClient(new Uri("http://localhost/"));
        }
    }
}
