using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMarket.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetLoggedUserId()
        {
            return Convert.ToInt32(HttpContext.User.Claims
                       .First(i => i.Type == "Id")?.Value);
        }
    }
}
