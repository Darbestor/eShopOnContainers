namespace Webhooks.API.Infrastructure;

public class WebhooksContext : DbContext
{

    public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    public DbSet<WebhookSubscription> Subscriptions { get; set; }
}

// public class WebhooksContextDesignFactory : IDesignTimeDbContextFactory<WebhooksContext>
// {
//     public WebhooksContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<WebhooksContext>()
//             .UseNpgsql("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.CatalogDb;Integrated Security=true");
//
//         return new WebhooksContext(optionsBuilder.Options);
//     }
// }
