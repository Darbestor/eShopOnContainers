using Catalog.API.Infrastructure.EntityConfigurations.Postgres;
using Catalog.API.Infrastructure.EntityConfigurations.SqlServer;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;

public class MsSqlCatalogContext : CatalogContext
{
    public MsSqlCatalogContext(DbContextOptions<MsSqlCatalogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MsSqlCatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new MsSqlCatalogItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new MsSqlCatalogTypeEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }
}


// public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
// {
//     public CatalogContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
//             .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.CatalogDb;Integrated Security=true");
//
//         return new CatalogContext(optionsBuilder.Options);
//     }
// }
