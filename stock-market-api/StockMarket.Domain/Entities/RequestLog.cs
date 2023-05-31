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
        public string Status { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public User User { get; set; }      
        public List<RequestLogTicker> RequestLogTickers { get; set; }
    }
}
