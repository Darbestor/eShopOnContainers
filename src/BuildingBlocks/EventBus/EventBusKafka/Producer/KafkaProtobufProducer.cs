
namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;

public class KafkaProtobufProducer<T>: IKafkaProtobufProducer<T>, IDisposable
    where T: class, IMessage<T>, new()
{
    private readonly ILogger<KafkaProtobufProducer<T>> _logger;
    private readonly IProducer<string,T> _producer;

    public KafkaProtobufProducer(ILogger<KafkaProtobufProducer<T>> logger, IKafkaProducerBuilder<T> producerBuilder)
    {
        if (producerBuilder == null)
        {
            throw new ArgumentNullException(nameof(producerBuilder));
        }

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _producer = producerBuilder.Build();
    }

    public void Produce(string topic, KafkaIntegrationEvent @event)
    {
        _logger.LogTrace("Kafka producing message '{MessageType}'", @event.Message.Descriptor.ClrType);
        var message = new Message<string, T>
        {
            Key = @event.Key, 
            Value = @event.Message as T, 
            Timestamp = new Timestamp(DateTime.UtcNow)
        };
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
