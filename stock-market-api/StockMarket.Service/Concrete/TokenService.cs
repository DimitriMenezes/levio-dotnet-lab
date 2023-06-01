using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using StockMarket.Domain.Entities;
using StockMarket.Service.Settings;

namespace StockMarket.Service.Concrete
{
    public static class TokenService
    {
        public static string GenerateToken(User client)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecuritySettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Name", client.Name.ToString()),  
                    new Claim("Email", client.Email.ToString()),
                    new Claim("Id", client.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }    

        public static string GenerateGuidToken()
        {
            var g = Guid.NewGuid();
            var guidString = Convert.ToBase64String(g.ToByteArray());
            var result = string.Join("", guidString.ToCharArray().Where(ch => Char.IsLetterOrDigit(ch)));
            return result;
        }
    }
}
