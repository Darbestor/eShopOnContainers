using System.Reflection;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Localization;

public static class Extensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddRedis(_ => configuration.GetRequiredConnectionString("redis"), "redis", tags: new[] { "ready", "liveness" });

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton(sp =>
        {
            var redisConfig = ConfigurationOptions.Parse(configuration.GetRequiredConnectionString("redis"), true);

            return ConnectionMultiplexer.Connect(redisConfig);
        });
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
            if (!config.Consumers.TryGetValue(KafkaConstants.CatalogTopicName, out var catalogConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.CatalogTopicName);
            }
            cluster.CreateTopicIfNotExists(KafkaConstants.BasketTopicName, 3, 1);
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.OrderingTopicName)
                    .WithName($"Basket.API-{KafkaConstants.OrderingTopicName}")
                    .WithConsumerConfig(orderingConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var orderingHandlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.IntegrationEvents.EventHandling.Ordering")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(orderingHandlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.CatalogTopicName)
                    .WithName($"Basket.API-{KafkaConstants.CatalogTopicName}")
                    .WithConsumerConfig(catalogConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var orderingHandlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.IntegrationEvents.EventHandling.Catalog")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(orderingHandlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
        });
        
        return services;
    }
}
