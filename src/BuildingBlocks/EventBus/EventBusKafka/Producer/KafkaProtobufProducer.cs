
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
        var policy = RetryPolicy.Handle<ProduceException<string, T>>()
            .Or<SocketException>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "Could not publish event: {Event} after {Timeout}s", topic, $"{time.TotalSeconds:n1}");
            });

        policy.Execute(() =>
        {
            _logger.LogTrace("Publishing event to Kafka: {EventId}", topic);

            _producer.Produce(topic, message, OnDeliveryReport);
        });
    }

    private void OnDeliveryReport(DeliveryReport<string, T> deliveryReport)
    {
        if (deliveryReport.Error.IsError)
        {
            _logger.LogError("Cannot produce message '{MessageId}' - {Reason}", deliveryReport.Value.Descriptor.ClrType, deliveryReport.Error.Reason);
        }
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
