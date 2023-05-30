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
    public class RealTimeTickerMapping : IEntityTypeConfiguration<RealTimeTicker>
    {        
        public void Configure(EntityTypeBuilder<RealTimeTicker> builder)
        {
            builder.Property(i => i.Current)
                .IsRequired();
        }
    }
}
