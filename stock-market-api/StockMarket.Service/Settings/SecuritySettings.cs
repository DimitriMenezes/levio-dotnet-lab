using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Settings
{
    public class SecuritySettings
    {
        public string HashSecret { get; set; }
        public int KeySize { get; set; }
        public int SaltSize { get; set; }
        public int Iteration { get; set; }
    }
}
