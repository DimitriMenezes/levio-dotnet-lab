using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Concrete
{
    public class EntrepriseRepository : BaseRepository<Entreprise>, IEntrepriseRepository
    {
        public EntrepriseRepository(DbContext context) : base(context)
        {
        }

        public async Task<Entreprise> GetByCode(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.Code == code);
        }

        public async Task<IQueryable<Entreprise>> GetByCodeList(List<string> codes)
        {
            return _dbSet.Where(i => codes.Contains(i.Code));
        }
    }
}
