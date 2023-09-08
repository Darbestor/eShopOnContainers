using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder
            .AddDbContextCheck<OrderingContext>(name: "OrderingDB-check",
                tags: new string[] { "ready" });

        return services;
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        static void ConfigurePgOptions(NpgsqlDbContextOptionsBuilder options)
        {
            options.MigrationsAssembly(typeof(Program).Assembly.FullName);

            // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 

            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
        };

        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseNpgsql(configuration.GetRequiredConnectionString("OrderingDB"), ConfigurePgOptions);
        });

        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseNpgsql(configuration.GetRequiredConnectionString("OrderingDB"), ConfigurePgOptions);
        });

        return services;
    }

    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
        services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderingSettings>(configuration);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        return services;
    }
}
