using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Context;
using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Concrete
{
    public class HistoricalTickerRepository : BaseRepository<HistoricalTicker>, IHistoricalTickerRepository
    {
        public HistoricalTickerRepository(StockMarketContext context) : base(context)
        {

        }

        public async Task<HistoricalTicker> GetExistingTicker(int entrepriseId, DateTime referenceDate)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.EntrepriseId == entrepriseId 
                && i.ReferenceDate == referenceDate);
        }
    }
}
