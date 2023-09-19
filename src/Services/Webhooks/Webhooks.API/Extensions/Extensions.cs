using System.Reflection;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Localization;
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
        return services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));
    }
    
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaFlow(configuration, (cluster, config) =>
        {
            if (!config.Consumers.TryGetValue(KafkaTopics.OrderStatus, out var orderStatusConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaTopics.OrderStatus);
            }
            if (!config.Consumers.TryGetValue(KafkaTopics.Catalog, out var catalogConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaTopics.Catalog);
            }

            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaTopics.OrderStatus)
                    .WithName($"Webhooks.API-{KafkaTopics.OrderStatus}")
                    .WithConsumerConfig(orderStatusConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace ==
                                    $"{rootNamespace}.IntegrationEvents.EventHandling.OrderStatus")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(handlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaTopics.Catalog)
                    .WithName($"Webhooks.API-{KafkaTopics.Catalog}")
                    .WithConsumerConfig(orderStatusConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace ==
                                    $"{rootNamespace}.IntegrationEvents.EventHandling.Catalog")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(handlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
        });

        return services;
    }
}
