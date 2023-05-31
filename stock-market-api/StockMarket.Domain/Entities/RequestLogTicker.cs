using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class RequestLogTicker : BaseEntity
    {
        public int RequestLogId { get; set; }
        public int TickerId { get; set; }   

        public Ticker Ticker { get; set; }
        public RequestLog RequestLog { get; set; }
    }
}
