using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Model.Filter
{
    public class TickerFilterModel
    {
        public int UserId { get; set; }
        public int EntrepriseId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
