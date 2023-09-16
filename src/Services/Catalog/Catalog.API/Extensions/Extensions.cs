using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Configuration;
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

    public static IServiceCollection AddKafkaFlow(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaSection = configuration.GetSection("Kafka");

        if (!kafkaSection.Exists())
        {
            return services;
        }

        KafkaConfig kafkaConfig = new();
        kafkaSection.Bind(kafkaConfig);

        services.AddKafka(builder =>
        {
            builder.UseMicrosoftLog();
            builder.AddCluster(cluster =>
            {
                cluster.WithBrokers(kafkaConfig.BootstrapServers);
                var hasSchema = kafkaConfig.SchemaRegistry is not null;
                if (hasSchema)
                {
                    cluster.WithSchemaRegistry(srb =>
                    {
                        srb.Url = kafkaConfig.SchemaRegistry.Url;
                        // foreach (var (key, value) in kafkaConfig.SchemaRegistry)
                        // {
                        //     srb.Set(key, value);
                        // }
                    });
                }

                // foreach (var producerConfig in kafkaConfig.Producers)
                // {
                //     cluster.CreateTopicIfNotExists(producerConfig.Topic, 3, 1);
                //     cluster.AddProducer($"{producerConfig.Topic}-producer", pb =>
                //     {
                //         pb.DefaultTopic(producerConfig.Topic)
                //             .WithProducerConfig(producerConfig);
                //         if (hasSchema)
                //         {
                //             pb.AddMiddlewares(x => x.AddSchemaRegistryProtobufSerializer(new ProtobufSerializerConfig()));
                //         }
                //     });
                // }
                cluster.CreateTopicIfNotExists("Catalog", 3, 1);
                cluster.AddProducer(nameof(ProductPriceChangedProtobuf), pb =>
                {
                    pb.DefaultTopic("Catalog");
                    if (hasSchema)
                    {
                        pb.AddMiddlewares(x => x.AddSchemaRegistryProtobufSerializer(
                            new ProtobufSerializerConfig
                            {
                                SubjectNameStrategy = SubjectNameStrategy.TopicRecord,
                                NormalizeSchemas = true,
                            }));
                    }
                });
                cluster.CreateTopicIfNotExists("Ordering", 3, 1);
                cluster.AddProducer("Ordering-producer", pb =>
                {
                    pb.DefaultTopic("Ordering");
                    if (hasSchema)
                    {
                        pb.AddMiddlewares(x => x.AddSchemaRegistryProtobufSerializer(
                            new ProtobufSerializerConfig { SubjectNameStrategy = SubjectNameStrategy.TopicRecord }));
                    }
                });
                cluster.AddConsumer(cb =>
                {
                    cb.Topic("Catalog")
                        .WithName("Catalog-consumer")
                        .WithGroupId("Catalog-consumer")
                        .WithBufferSize(100)
                        .WithWorkersCount(1)
                        .WithAutoOffsetReset(AutoOffsetReset.Earliest);
                    cb.AddMiddlewares(m =>
                    {
                        if (hasSchema)
                        {
                            m.AddSchemaRegistryProtobufCustomSerializer();
                        }

                        m.AddTypedHandlers(x => x.WhenNoHandlerFound(mc =>
                        {
                            Console.WriteLine("No handler binded for Catalog");
                        }).AddHandler<ProductPriceEventHandlerKafkaFlow>());
                    });
                });
                cluster.AddConsumer(cb =>
                {
                    cb.Topic("Ordering")
                        .WithName("Ordering-consumer")
                        .WithGroupId("Ordering-consumer")
                        .WithBufferSize(100)
                        .WithWorkersCount(1)
                        .WithAutoOffsetReset(AutoOffsetReset.Earliest);
                    cb.AddMiddlewares(m =>
                    {
                        if (hasSchema)
                        {
                            m.AddSchemaRegistryProtobufCustomSerializer();
                        }

                        m.AddTypedHandlers(x => x.WhenNoHandlerFound(mc =>
                        {
                            Console.WriteLine("No handler binded for Ordering");
                        }).AddHandler<OrderStockRejectedIntegrationEventProtoHandler>());
                    });
                });

                // foreach (var consumerConfig in kafkaConfig.Consumers)
                // {
                //     cluster.AddConsumer(cb =>
                //     {
                //         cb.Topic(consumerConfig.Topic)
                //             .WithName($"{consumerConfig.Topic}-consumer")
                //             .WithConsumerConfig(consumerConfig)
                //             .WithBufferSize(100)
                //             .WithWorkersCount(1);
                //         if (hasSchema)
                //         {
                //             cb.AddMiddlewares(m =>
                //             {
                //                 m.AddSchemaRegistryProtobufSerializer();
                //             });
                //         }
                //     });
                // }
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
