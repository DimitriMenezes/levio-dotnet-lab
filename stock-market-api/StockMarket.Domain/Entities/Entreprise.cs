using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class Entreprise : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<Ticker> Tickers { get; set; }
    }
}
