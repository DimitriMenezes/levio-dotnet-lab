using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StockMarket.Service.Model
{
    public class RealTimeStockApiResultModel
    {       
        public List<RealTimeTickerResultModel> Data { get; set; }
    }
   
    public class RealTimeTickerResultModel
    {
        public string Ticker { get; set; }
        [JsonProperty("last_trade_time")]
        public DateTime Date { get; set; }
        [JsonProperty("price")]
        public decimal Current { get; set; }
        [JsonProperty("day_open")]
        public decimal Open { get; set; }
        [JsonProperty("day_high")]
        public decimal High { get; set; }
        [JsonProperty("day_low")]
        public decimal Low { get; set; }       
    }
}
