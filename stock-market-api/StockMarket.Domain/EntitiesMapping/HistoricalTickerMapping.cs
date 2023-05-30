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
    public class HistoricalTickerMapping : IEntityTypeConfiguration<HistoricalTicker>
    {        
        public void Configure(EntityTypeBuilder<HistoricalTicker> builder)
        {
            builder.Property(i => i.Close)
                .IsRequired();

            builder.Property(i => i.Volume)
               .IsRequired();
        }
    }
}
