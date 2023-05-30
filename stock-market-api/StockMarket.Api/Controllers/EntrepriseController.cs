using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockMarket.Service.Abstract;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMarket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntrepriseController : ControllerBase
    {
        private readonly ILogger<EntrepriseController> _logger;
        private readonly IEntrepriseService _entrepriseService;

        public EntrepriseController(IEntrepriseService entrepriseService,
            ILogger<EntrepriseController> logger)
        {
            _entrepriseService = entrepriseService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(EntrepriseModel model)
        {
            var result = await _entrepriseService.SaveEntreprise(model);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }
    }
}
