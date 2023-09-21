using Hangfire;
using Hangfire.PostgreSql;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Options;
using Ordering.BackgroundTasks.Hangfire;
using Ordering.BackgroundTasks.IntegrationEvents.EventHandling;

namespace Ordering.BackgroundTasks.Extensions;

public static class CustomExtensionMethods
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder
            .AddNpgSql(_ =>
                {
                    var conn = configuration.GetRequiredConnectionString("OrderingDB");
                    return conn + (conn.EndsWith(';') ? "" : ";") + "Pooling=false";
                },
                name: "OrderingTaskDB-check",
                tags: new[] { "live", "ready" });

        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.Configure<BackgroundTaskSettings>(configuration)
            .Configure<BackgroundTaskSettings>(o =>
            {
                o.ConnectionString = configuration.GetRequiredConnectionString("OrderingDB");
            });
    }

    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddKafkaFlow(configuration, (cluster, config) =>
        {
            if (!config.Consumers.TryGetValue(KafkaTopics.OrderStatus, out var consumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaTopics.OrderStatus);
            }

            cluster.CreateTopicIfNotExists(KafkaTopics.OrderGracePeriod, 3, 1);
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaTopics.OrderStatus)
                    .WithName($"Ordering.BackgroundTasks-{KafkaTopics.OrderStatus}")
                    .WithConsumerConfig(consumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandler<UserCheckoutAcceptedIntegrationEventHandler>()
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
        });
    }

    public static IServiceCollection AddHangfireServer(this IServiceCollection services)
    {
        return services.AddSingleton<IHangfireServer, HangfireServer>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfigurationRoot>();

            GlobalConfiguration.Configuration.UsePostgreSqlStorage(configuration.GetConnectionString("OrderingDB"))
                .UseColouredConsoleLogProvider()
                .UseActivator(new HangfireJobActivator(sp));

            return new HangfireServer(sp.GetRequiredService<ILogger<HangfireServer>>(),
                sp.GetRequiredService<IOptions<BackgroundTaskSettings>>());
        });
    }
}
