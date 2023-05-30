using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<RequestLog> RequestLogs { get; set; }
    }
}
