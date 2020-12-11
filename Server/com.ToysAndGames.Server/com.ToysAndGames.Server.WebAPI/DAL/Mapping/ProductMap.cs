using com.ToysAndGames.Server.WebAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.ToysAndGames.Server.WebAPI.DAL.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(100);

            builder.Property(e => e.AgeRestriction)
                .HasMaxLength(3);

            builder.Property(e => e.Company)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Price)
                .HasMaxLength(4);
        }
    }
}
