using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private StockMarketContext _context;
        private IUserRepository _userRepository;
        private IEntrepriseRepository _entrepriseRepository;
        private IHistoricalTickerRepository _historicalTickerRepository;
        private IRealTimeTickerRepository _realTimeTickerRepository;
        private IRequestLogRepository _requestLogRepository;
        private IRequestLogTickerRepository _requestLogTickerRepository;

        public UnitOfWork(StockMarketContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public IEntrepriseRepository EntrepriseRepository
        {
            get
            {
                if (_entrepriseRepository == null)
                {
                    _entrepriseRepository = new EntrepriseRepository(_context);
                }
                return _entrepriseRepository;
            }
        }

        public IHistoricalTickerRepository HistoricalTickerRepository
        {
            get
            {
                if (_historicalTickerRepository == null)
                {
                    _historicalTickerRepository = new HistoricalTickerRepository(_context);
                }
                return _historicalTickerRepository;
            }
        }

        public IRealTimeTickerRepository RealTimeTickerRepository
        {
            get
            {
                if (_realTimeTickerRepository == null)
                {
                    _realTimeTickerRepository = new RealTimeTickerRepository(_context);
                }
                return _realTimeTickerRepository;
            }
        }

        public IRequestLogRepository RequestLogRepository
        {
            get
            {
                if (_requestLogRepository == null)
                {
                    _requestLogRepository = new RequestLogRepository(_context);
                }
                return _requestLogRepository;
            }
        }

        public IRequestLogTickerRepository RequestLogTickerRepository
        {
            get
            {
                if (_requestLogTickerRepository == null)
                {
                    _requestLogTickerRepository = new RequestLogTickerRepository(_context);
                }
                return _requestLogTickerRepository;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}