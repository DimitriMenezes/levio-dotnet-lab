﻿using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Abstract
{
    public interface IHistoricalTickerRepository : IBaseRepository<HistoricalTicker>
    {
        Task<HistoricalTicker> GetExistingTicker(int entrepriseId, DateTime referenceDate);
    }
}
