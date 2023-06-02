using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface ISecurityService
    {
        string GenerateToken(User client);
        string EncodePassword(string password);
        bool ValidatePassword(string salt, string hashedPassword, string password);
    }
}
