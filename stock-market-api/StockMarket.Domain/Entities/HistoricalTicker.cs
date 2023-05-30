using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class HistoricalTicker : Ticker
    {
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }
}
