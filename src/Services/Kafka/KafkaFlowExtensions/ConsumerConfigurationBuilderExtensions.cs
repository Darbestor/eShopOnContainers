using Confluent.SchemaRegistry;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer.SchemaRegistry;
using Microsoft.eShopOnContainers.Kafka.Consumers;

namespace Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;

/// <summary>
/// No needed
/// </summary>
public static class ConsumerConfigurationBuilderExtensions
{
    /// <summary>
    /// Registers a middleware to deserialize protobuf messages using schema registry
    /// </summary>
    /// <param name="middlewares">The middleware configuration builder</param>
    /// <returns></returns>
    public static IConsumerMiddlewareConfigurationBuilder AddSchemaRegistryProtobufCustomSerializer(
        this IConsumerMiddlewareConfigurationBuilder middlewares)
    {
        return middlewares.Add(
            resolver => new DeserializerConsumerMiddleware(
                new ConfluentProtobufSerializer(resolver),
                new SchemaRegistryTypeResolver(
                    new ProtobufTypeNameResolver(resolver.Resolve<ISchemaRegistryClient>()))));
    }
}
