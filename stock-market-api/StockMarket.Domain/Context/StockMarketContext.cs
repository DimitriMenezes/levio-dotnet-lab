using Microsoft.EntityFrameworkCore;
using StockMarket.Domain.Entities;
using StockMarket.Domain.EntitiesMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.Context
{
    public class StockMarketContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Entreprise> Entreprise { get; set; }
        public DbSet<Ticker> Ticker { get; set; }
        public DbSet<RequestLog> RequestLog { get; set; }
        public DbSet<RequestLogTicker> RequestLogTicker { get; set; }

        public StockMarketContext()
        {

        }
        public StockMarketContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new EntrepriseMapping());
            modelBuilder.ApplyConfiguration(new TickerMapping());
            modelBuilder.ApplyConfiguration(new HistoricalTickerMapping());
            modelBuilder.ApplyConfiguration(new RealTimeTickerMapping());
            modelBuilder.ApplyConfiguration(new RequestLogMapping());
            modelBuilder.ApplyConfiguration(new RequestLogItemMapping());
        }
    }
}
