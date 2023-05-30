using Microsoft.AspNetCore.Mvc;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetUserById(id);            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserModel model)
        {
            var result = await _userService.CreateUser(model);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserModel model)
        {
            var result = await _userService.UpdateUser(model);
            if (result.Errors != null)
                return BadRequest(result.Errors);
            return Ok(result);
        }
    }
}
