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
    public class EntrepriseMapping : IEntityTypeConfiguration<Entreprise>
    {
        public void Configure(EntityTypeBuilder<Entreprise> builder)
        {
            builder.HasIndex(i => i.Code)
                .IsUnique();

            builder.Property(i => i.Code)
                .IsRequired()                
                .HasMaxLength(20);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
