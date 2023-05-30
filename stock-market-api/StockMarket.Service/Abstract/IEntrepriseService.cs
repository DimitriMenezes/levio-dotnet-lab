using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Abstract
{
    public interface IEntrepriseService
    {
        Task<ResultModel> SaveEntreprise(EntrepriseModel model);
    }
}
