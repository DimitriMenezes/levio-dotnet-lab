using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Abstract
{
    public interface IEntrepriseRepository : IBaseRepository<Entreprise>
    {
        Task<Entreprise> GetByCode(string code);
        Task<IQueryable<Entreprise>> GetByCodeList(List<string> codes);
    }
}
