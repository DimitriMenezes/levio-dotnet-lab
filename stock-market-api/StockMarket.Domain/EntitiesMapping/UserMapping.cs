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
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(i => i.Email)
                .IsUnique();

            builder.Property(i => i.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(i => i.Email)
               .HasMaxLength(200)
               .IsRequired();

            builder.Property(i => i.Password)
                .HasMaxLength(500)
               .IsRequired();
        }
    }
}
