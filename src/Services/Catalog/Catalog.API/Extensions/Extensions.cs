using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Consumers;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.Configuration;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.TempIntegrationStructures;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Extensions;

public static class Extensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder
            .AddDbContextCheck<CatalogContext>(name: "CatalogDB-check", tags: new string[] { "ready" });

        var accountName = configuration["AzureStorageAccountName"];
        var accountKey = configuration["AzureStorageAccountKey"];

        if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(accountKey))
        {
            hcBuilder
                .AddAzureBlobStorage(
                    $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey};EndpointSuffix=core.windows.net",
                    name: "catalog-storage-check",
                    tags: new string[] { "ready" });
        }

        return services;
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        static void ConfigureNpgsqlOptions(NpgsqlDbContextOptionsBuilder options)
        {
            options.MigrationsAssembly(typeof(Program).Assembly.FullName);

            // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 

            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        }

        ;

        services.AddDbContext<CatalogContext>(options =>
        {
            var connectionString = configuration.GetRequiredConnectionString("CatalogDB");

            options.UseNpgsql(connectionString, ConfigureNpgsqlOptions);
        });

        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            var connectionString = configuration.GetRequiredConnectionString("CatalogDB");

            options.UseNpgsql(connectionString, ConfigureNpgsqlOptions);
        });


        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CatalogSettings>(configuration);

        // TODO: Move to the new problem details middleware
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

    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaFlow(configuration, (cluster, config) =>
        {
            if (!config.Consumers.TryGetValue(KafkaConstants.OrderingTopicName, out var orderingConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.OrderingTopicName);
            }
            cluster.CreateTopicIfNotExists(KafkaConstants.CatalogTopicName, 3, 1);
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.OrderingTopicName)
                    .WithName($"Catalog.API-{KafkaConstants.OrderingTopicName}")
                    .WithConsumerConfig(orderingConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandler<ProductPriceEventHandlerKafkaFlow>());
                });
            });
        });
        
        return services;
    }

    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
            sp => (DbConnection c) => new IntegrationEventLogService(c));

        services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        return services;
    }
}
