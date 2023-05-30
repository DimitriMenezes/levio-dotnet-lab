using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class RequestLog : BaseEntity
    {        
        public int UserId { get; set; }
        public int TickerId { get; set; }
        public User User { get; set; }
        public Ticker Ticker { get; set; }
    }
}
