using Catalog.API.Infrastructure.EntityConfigurations.Postgres;
using Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

namespace Catalog.API.Infrastructure;

public class PostgresCatalogContext : DbContext
{
    public PostgresCatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogBrand> CatalogBrands { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PostgreCatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new PostgreCatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new PostgreCatalogItemEntityTypeConfiguration());
    }
}
