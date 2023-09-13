using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;

public class KafkaProtobufProducer<T>: IKafkaProtobufProducer<T>, IDisposable
    where T: class, IMessage<T>, new()
{
    private readonly IProducer<string,T> _producer;

    public KafkaProtobufProducer(IKafkaPersistentConnection persistentConnection)
    {
        var schemaRegistryConfig = new CachedSchemaRegistryClient(persistentConnection.KafkaConfig.SchemaRegistry);
        _producer = new DependentProducerBuilder<string, T>(persistentConnection.Handle)
            .SetValueSerializer(new ProtobufSerializer<T>(schemaRegistryConfig).AsSyncOverAsync())
            .Build();
    }

    public void Produce(string topic, KafkaIntegrationEvent @event)
    {
        var message = new Message<string, T> 
            { Key = @event.Key, Value = @event.Message as T };
        _producer.Produce(topic, message);
    }

    public void Dispose()
    {
        if (_producer != null)
        {
            _producer.Flush();
            _producer?.Dispose();
        }    
    }
}
