using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using StockMarket.Domain.Entities;
using StockMarket.Service.Settings;
using StockMarket.Service.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace StockMarket.Service.Concrete
{
    public class SecurityService : ISecurityService
    {        
        private readonly SecuritySettings _security;
        public SecurityService(IConfiguration configuration)
        {            
            var keyVaultUrl = configuration.GetSection("KeyVaultUrl").Get<string>();
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            _security = new SecuritySettings
            {
                HashSecret = client.GetSecret("HashSecret").Value.Value,
                Iteration = Convert.ToInt32(client.GetSecret("Iteration").Value.Value),            
                KeySize = Convert.ToInt32(client.GetSecret("KeySize").Value.Value),
                SaltSize = Convert.ToInt32(client.GetSecret("SaltSize").Value.Value)
            };
        }

        public string GenerateToken(User client)
        {            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_security.HashSecret);
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

        public string EncodePassword(string password)
        {
            var keySize = _security.KeySize;
            var saltSize = _security.SaltSize;
            var iteration = _security.Iteration;

            using var algorithm = new Rfc2898DeriveBytes(password, saltSize, iteration, HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
            var salt = Convert.ToBase64String(algorithm.Salt);
            return $"{Convert.ToBase64String(Encoding.ASCII.GetBytes(iteration.ToString()))}.{salt}.{key}";
        }

        public bool ValidatePassword(string salt, string hashedPassword, string password)
        {
            var keySize = _security.KeySize;            
            var iteration = _security.Iteration;

            using var algorithm = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iteration, HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(keySize));

            return key.Equals(hashedPassword);
        }
    }
}
