using com.ToysAndGames.Server.WebAPI.DAL.Mapping;
using com.ToysAndGames.Server.WebAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.ToysAndGames.Server.WebAPI.DAL.Context
{
    public partial class ApiContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ApiDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductMap());

            // Lets keep our mapping files clean
            modelBuilder.Entity<Product>().HasData(GetProductSeed());
        }

        private Product[] GetProductSeed()
        {
            return new Product[]{
                new Product
                {
                    Id = 1,
                    Name = "Our awesome toy  for kids",
                    AgeRestriction = 10,
                    Company = "That brand",
                    Description = "Everyone should try this toy",
                    Price = 99
                },
                new Product
                {
                    Id = 2,
                    Name = "Our awesome game  for adults",
                    AgeRestriction = 21,
                    Company = "The other brand",
                    Description = "Everyone should try this game",
                    Price = 199
                }
            };
        }
    }
}
