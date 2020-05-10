using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Database.Contexts
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            ConfigurePrimaryKeys(modelBuilder);
            ConfigureForeignKeys(modelBuilder);
            ConfigureConsistency(modelBuilder);
        }

        private void ConfigurePrimaryKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricedProductEntity>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<SalesAggregateEntity>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => p.Id);
            
            modelBuilder.Entity<SeasonPricedProductEntity>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Season>()
                .HasKey(p=> p.Id);

            modelBuilder.Entity<PricedProduct>()
                .Property(p => p.CreatedOn)
                .HasValueGenerator<DateTimeGenerator>();
        }

        private void ConfigureForeignKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricedProductEntity>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(k => k.ProductGuid);
            
            modelBuilder.Entity<SalesAggregateEntity>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(k => k.ProductGuid);

            modelBuilder.Entity<SeasonPricedProductEntity>()
                .HasOne(o => o.PricedProductEntity)
                .WithMany()
                .HasForeignKey(k => k.PricedProductEntityId);
        }

        private void ConfigureConsistency(ModelBuilder modelBuilder)
        {
        }

        public DbSet<PricedProductEntity> PricedProducts { get; set; }
        public DbSet<SalesAggregateEntity> SaleAggregates { get; set; }
        
        public DbSet<SeasonEntity> Seasons { get; set; }
        
        public DbSet<SeasonPricedProductEntity> SeasonPricedProducts { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<AdvertisedProductEntity> Advertised { get; set; }

        public class DateTimeGenerator : ValueGenerator<DateTime>
        {
            public override DateTime Next(EntityEntry entry)
            {
                return DateTime.Now;
            }

            public override bool GeneratesTemporaryValues { get; }
        }
    }
}