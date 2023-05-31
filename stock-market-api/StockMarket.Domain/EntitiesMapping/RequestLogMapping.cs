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
    public class RequestLogMapping : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.HasOne(i => i.User)
                .WithMany(i => i.RequestLogs)
                .HasForeignKey(i => i.UserId);

            builder.Property(i => i.RequestJson)
                .IsRequired();
            builder.Property(i => i.ResponseJson)
                .IsRequired();
            builder.Property(i => i.Status)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
