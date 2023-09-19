using System.Reflection;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;
using Microsoft.Extensions.Localization;
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
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
            sp => (DbConnection c) => new IntegrationEventLogService(c));

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
    
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaFlow(configuration, (cluster, config) =>
        {
            if (!config.Consumers.TryGetValue(KafkaConstants.OrderGracePeriodTopicName, out var gracePeriodConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.OrderGracePeriodTopicName);
            }
            if (!config.Consumers.TryGetValue(KafkaConstants.BasketTopicName, out var basketConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.BasketTopicName);
            }
            if (!config.Consumers.TryGetValue(KafkaConstants.OrderStockTopicName, out var stockConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.OrderStockTopicName);
            }
            if (!config.Consumers.TryGetValue(KafkaConstants.OrderPaymentTopicName, out var paymentConsumerConfig))
            {
                throw new ArgumentException("Kafka consumer '{Name}' not found in the configuration",
                    KafkaConstants.OrderPaymentTopicName);
            }
            
            cluster.CreateTopicIfNotExists(KafkaConstants.OrderingTopicName, 3, 1);
            cluster.CreateTopicIfNotExists(KafkaConstants.OrderStatusTopicName, 10, 3);
            
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.BasketTopicName)
                    .WithName($"Ordering.API-{KafkaConstants.BasketTopicName}")
                    .WithConsumerConfig(basketConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.Application.IntegrationEvents.EventHandling.Basket")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(handlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.OrderGracePeriodTopicName)
                    .WithName($"Ordering.API-{KafkaConstants.OrderGracePeriodTopicName}")
                    .WithConsumerConfig(basketConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.Application.IntegrationEvents.EventHandling.GracePeriod")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(handlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.OrderStockTopicName)
                    .WithName($"Ordering.API-{KafkaConstants.OrderStockTopicName}")
                    .WithConsumerConfig(basketConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.Application.IntegrationEvents.EventHandling.Stock")
                        .ToArray();
                    m.AddSchemaRegistryProtobufCustomSerializer()
                        .AddTypedHandlers(x => x.AddNoHandlerFoundLogging()
                            .AddHandlersFromAssemblyOf(handlerTypes)
                            .WithHandlerLifetime(InstanceLifetime.Transient));
                });
            });
            cluster.AddConsumer(cb =>
            {
                cb.Topic(KafkaConstants.OrderPaymentTopicName)
                    .WithName($"Ordering.API-{KafkaConstants.OrderPaymentTopicName}")
                    .WithConsumerConfig(basketConsumerConfig)
                    .WithBufferSize(100)
                    .WithWorkersCount(1)
                    .WithAutoOffsetReset(AutoOffsetReset.Latest)
                    .WithManualStoreOffsets();
                cb.AddMiddlewares(m =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var rootNamespace = assembly.GetCustomAttribute<RootNamespaceAttribute>()!.RootNamespace;
                    var handlerTypes = assembly.GetTypes()
                        .Where(x => x.Namespace == $"{rootNamespace}.Application.IntegrationEvents.EventHandling.Payment")
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
