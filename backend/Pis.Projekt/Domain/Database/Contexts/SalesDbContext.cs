using Microsoft.EntityFrameworkCore;
using Pis.Projekt.Business.Scheduling;

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
        }

        private void ConfigureForeignKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricedProductEntity>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(k => k.ProductGuid);
            modelBuilder.Entity<SalesAggregateEntity>()
                .HasOne(o => o.PricedProduct)
                .WithMany()
                .HasForeignKey(k => k.PricedProductGuid);
        }

        private void ConfigureConsistency(ModelBuilder modelBuilder)
        {
            // TODO
        }

        public DbSet<PricedProductEntity> PricedProducts { get; set; }
        public DbSet<SalesAggregateEntity> SaleAggregates { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
    }
}