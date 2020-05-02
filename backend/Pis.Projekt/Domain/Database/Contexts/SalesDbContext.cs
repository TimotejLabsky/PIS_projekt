using Microsoft.EntityFrameworkCore;
using Pis.Projekt.Business.Scheduling;

namespace Pis.Projekt.Domain.Database.Contexts
{
    public class SalesDbContext: DbContext
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
                .HasKey(p => p.SalesWeek);
            modelBuilder.Entity<SalesAggregateEntity>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<UserTaskEntity>()
                .HasKey(p => p.Id);
        }
        private void ConfigureForeignKeys(ModelBuilder modelBuilder)
        {
            // TODO
            modelBuilder.Entity<PricedProductEntity>()
                .Property(p => p.Product);
            modelBuilder.Entity<SalesAggregateEntity>();
            modelBuilder.Entity<ProductEntity>();
            modelBuilder.Entity<UserTaskEntity>();
        }
        private void ConfigureConsistency(ModelBuilder modelBuilder)
        {
            // TODO
        }

        public DbSet<PricedProductEntity> PricedProducts { get; set; }
        public DbSet<SalesAggregateEntity> SaleAggregates { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<UserTaskEntity> UserTasks { get; set; }
    }
}