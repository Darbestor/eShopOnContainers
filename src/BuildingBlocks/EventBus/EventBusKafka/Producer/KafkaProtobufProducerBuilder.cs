using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;

public class KafkaProtobufProducerBuilder<T>: IKafkaProducerBuilder<T>
where T: class, IMessage<T>, new()
{
    private readonly IKafkaPersistentConnection _persistentConnection;

    public KafkaProtobufProducerBuilder(IKafkaPersistentConnection persistentConnection)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
    }
    
    public IProducer<string, T> Build()
    {
        var schemaRegistryConfig = new CachedSchemaRegistryClient(_persistentConnection.KafkaConfig.SchemaRegistry);
        return new DependentProducerBuilder<string, T>(_persistentConnection.Handle)
            .SetValueSerializer(new ProtobufSerializer<T>(schemaRegistryConfig).AsSyncOverAsync())
            .Build();
    }
}
