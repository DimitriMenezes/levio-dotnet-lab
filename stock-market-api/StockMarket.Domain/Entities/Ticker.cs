using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class Ticker : BaseEntity
    {
        public int EntrepriseId { get; set; }
        public DateTime ReferenceDate { get; set; }        
        public decimal Open { get; set; }        
        public decimal High { get; set; }
        public decimal Low { get; set; }        
        public Entreprise Entreprise { get; set; }
        public List<RequestLog> RequestLogs { get; set; }
    }
}
