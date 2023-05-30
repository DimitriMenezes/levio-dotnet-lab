using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Domain.EntitiesMapping
{
    public class TickerMapping : IEntityTypeConfiguration<Ticker>
    {
        public void Configure(EntityTypeBuilder<Ticker> builder)
        {
            builder
                .HasDiscriminator<string>("Type")
                .HasValue<RealTimeTicker>("RealTime")
                .HasValue<HistoricalTicker>("Historical");

            builder.Property(i => i.ReferenceDate)
                .IsRequired();           
            builder.Property(i => i.Open)
                 .IsRequired();
            builder.Property(i => i.High)
                 .IsRequired();           
            builder.Property(i => i.Low)
                .IsRequired();

            builder.HasOne(i => i.Entreprise)
                .WithMany(i => i.Tickers)
                .HasForeignKey(i => i.EntrepriseId);            
        }
    }
}
