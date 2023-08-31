using Catalog.API.Infrastructure.EntityConfigurations.Postgres;
using Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

namespace Catalog.API.Infrastructure;

public class PostgresCatalogContext : CatalogContext
{
    public PostgresCatalogContext(DbContextOptions<PostgresCatalogContext> options) : base(options)
    {
    }

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
        base.OnModelCreating(builder);
    }
}
