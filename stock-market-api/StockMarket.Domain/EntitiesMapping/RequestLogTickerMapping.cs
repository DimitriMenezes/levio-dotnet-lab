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
    public class RequestLogItemMapping : IEntityTypeConfiguration<RequestLogTicker>
    {
        public void Configure(EntityTypeBuilder<RequestLogTicker> builder)
        {
            builder.Ignore(i => i.Id);
            builder.Ignore(i => i.CreatedAt);

            builder.HasKey(i => new { i.RequestLogId, i.TickerId });

            builder.HasOne(i => i.Ticker)
                .WithMany(i => i.RequestLogTickers)
                .HasForeignKey(i => i.TickerId);

            builder.HasOne(i => i.RequestLog)
               .WithMany(i => i.RequestLogTickers)
               .HasForeignKey(i => i.RequestLogId);
        }
    }
}
