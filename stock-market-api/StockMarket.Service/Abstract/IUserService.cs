using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface IUserService
    {
        Task<ResultModel> GetUserById(int id);
        Task<ResultModel> CreateUser(UserModel model);
        Task<ResultModel> UpdateUser(UserModel model);
    }
}
