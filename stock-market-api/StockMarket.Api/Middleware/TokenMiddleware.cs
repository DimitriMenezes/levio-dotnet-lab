using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace StockMarket.Api.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var userLogged = new UserLogged();
                var jwtEncodedString = authorizationHeader[7..];
                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                
                userLogged.Id = Convert.ToInt32(token.Claims.FirstOrDefault(i => i.Type == "Id").Value);
                userLogged.Email = token.Claims.FirstOrDefault(i => i.Type == "Email").Value;
                userLogged.Name = token.Claims.FirstOrDefault(i => i.Type == "Name").Value;                
            }

            await _next(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
