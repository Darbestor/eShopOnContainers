using System.Reflection;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.eShopOnContainers.Payment.API.Extensions;

public static class CustomExtensionMethods
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaFlow(configuration, (cluster, config) =>
        {
            if (!config.Consumers.TryGetValue(KafkaTopics.OrderStatus, out var consumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaTopics.OrderStatus);
            }

            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaTopics.OrderStatus)
                    .WithName($"Payment.API-{KafkaTopics.OrderStatus}")
                    .WithConsumerConfig(consumerConfig)
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
                                    $"{rootNamespace}.IntegrationEvents.EventHandling")
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
