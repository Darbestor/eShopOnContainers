namespace Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    protected CatalogContext(DbContextOptions options) : base(options)
    {}

    public virtual DbSet<CatalogItem> CatalogItems { get; set; }
    public virtual DbSet<CatalogBrand> CatalogBrands { get; set; }
    public virtual DbSet<CatalogType> CatalogTypes { get; set; }
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
