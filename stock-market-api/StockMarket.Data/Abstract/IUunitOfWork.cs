using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Abstract
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRequestLogTickerRepository RequestLogTickerRepository { get; }
        IEntrepriseRepository EntrepriseRepository { get; }
        IHistoricalTickerRepository HistoricalTickerRepository { get; }
        IRealTimeTickerRepository RealTimeTickerRepository { get; }
        IRequestLogRepository RequestLogRepository { get; }
        Task SaveChanges();
        void Rollback();
    }
}
