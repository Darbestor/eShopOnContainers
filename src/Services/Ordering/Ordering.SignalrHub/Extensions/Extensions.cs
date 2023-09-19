using System.Reflection;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Localization;

internal static class Extensions
{
    public static IServiceCollection AddSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetConnectionString("redis") is string redisConnection)
        {
            // TODO: Add a redis health check
            services.AddSignalR().AddStackExchangeRedis(redisConnection);
        }
        else
        {
            services.AddSignalR();
        }

        // Configure hub auth (grab the token from the query string)
        return services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var endpoint = context.HttpContext.GetEndpoint();

                    // Make sure this is a Hub endpoint.
                    if (endpoint?.Metadata.GetMetadata<HubMetadata>() is null)
                    {
                        return Task.CompletedTask;
                    }

                    context.Token = accessToken;

                    return Task.CompletedTask;
                }
            };
        });
    }
    
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
                    .WithName($"Ordering.SignalR-{KafkaTopics.OrderStatus}")
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
                                    $"{rootNamespace}.Application.IntegrationEvents.EventHandling")
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
