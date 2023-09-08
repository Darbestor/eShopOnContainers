using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        void ConfigurePgOptions(NpgsqlDbContextOptionsBuilder options)
        {
            options.MigrationsAssembly(typeof(Program).Assembly.FullName);

            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            
        }

        services.AddDbContext<WebhooksContext>(options =>
        {
            options.UseNpgsql(configuration.GetRequiredConnectionString("WebHooksDB"),
                npgsqlOptionsAction: ConfigurePgOptions);
        });
        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseNpgsql(configuration.GetRequiredConnectionString("WebHooksDB"),
                npgsqlOptionsAction: ConfigurePgOptions);
        });
        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder
            .AddDbContextCheck<WebhooksContext>(
            name: "WebhooksApiDb-check",
            tags: new string[] { "ready" });

        return services;
    }

    public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
    {
        // Add http client services
        services.AddHttpClient("GrantClient")
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        return services;
    }

    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        return services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
    }
}
