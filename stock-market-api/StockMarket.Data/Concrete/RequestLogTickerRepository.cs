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
    public class RequestLogTickerRepository : BaseRepository<RequestLogTicker>, IRequestLogTickerRepository
    {
        public RequestLogTickerRepository(DbContext context) : base(context)
        {
        }
    }
}
