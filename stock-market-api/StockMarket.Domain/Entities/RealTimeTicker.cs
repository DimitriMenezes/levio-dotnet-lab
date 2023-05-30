using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class RealTimeTicker : Ticker
    {        
        public decimal Current { get; set; }       
    }
}
