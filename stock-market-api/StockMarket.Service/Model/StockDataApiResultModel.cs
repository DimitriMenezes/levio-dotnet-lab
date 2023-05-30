using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Model
{
    public class StockDataApiResultModel
    {
        public MetaDataResultModel Meta { get; set; }
        public List<TickerResultModel> Data { get; set; }
    }

    public class MetaDataResultModel
    {
        public string Tikcer { get; set; }
        public string Name { get; set; }
        public int Found { get; set; }
        public int Returned { get; set; }     
    }

    public class TickerResultModel
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }      
    }
}
