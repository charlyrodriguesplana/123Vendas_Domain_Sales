using _123Vendas.Domain.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Domain.Sales.Database
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }

        //Em um projeto maior ou real, eu criaria arquivos especificos de configuração para cada tabela.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SaleNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.BranchName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired();

                entity.OwnsMany(e => e.Items, items =>
                {
                    items.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
                    items.Property(i => i.UnitPrice).HasPrecision(18, 2);
                    items.Property(i => i.DiscountPercent).HasPrecision(5, 2);
                });

                entity.Ignore(e => e.Events);
            });
        }
    }
}
