using Google.Protobuf;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public interface IKafkaManager
{
    void Publish(string key, KafkaIntegrationEvent @event);
    void Subscribe<T>(string topicName)
        where T : class, IMessage<T>, new();

    public void Unsubscribe<T>(string topicName)
        where T : class, IMessage<T>, new();
}

public class KafkaManager : IKafkaManager
{
    private readonly IKafkaPersistentConnection _persistentConnection;
    private readonly ILogger<KafkaManager> _logger;
    private readonly IConsumerManager _consumerManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly int _retryCount;
    
    public KafkaManager(IKafkaPersistentConnection persistentConnection, ILogger<KafkaManager> logger,
        IConsumerManager consumerManager,
        IServiceProvider serviceProvider,
        int retryCount = 5)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumerManager = consumerManager ?? throw new ArgumentNullException(nameof(consumerManager));
        _serviceProvider = serviceProvider;
        _retryCount = retryCount;
    }


    public void Publish(string key, KafkaIntegrationEvent @event)
    {
        var eventType = @event.Message.Descriptor.ClrType;
        using var scope = _serviceProvider.CreateScope();
        var type = typeof(IKafkaProtobufProducer<>).MakeGenericType(eventType);
        if (scope.ServiceProvider.GetService(type) is not IKafkaProtobufProducer producer)
        {
            _logger.LogError("Kafka Producer for {EventName} not registered", @event.Message.GetGenericTypeName());
            return;
        }
        
        var policy = RetryPolicy.Handle<ProduceException<string, string>>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "Could not publish event: {Event} after {Timeout}s", key, $"{time.TotalSeconds:n1}");
            });

        policy.Execute(() =>
        {
            _logger.LogTrace("Publishing event to Kafka: {EventId}", key);

            producer.Produce(_persistentConnection.KafkaConfig.Producer.Topic, @event);
        });
    }

    public void Subscribe<T>(string topicName)
    where T: class, IMessage<T>, new()
    {
        _consumerManager.Subscribe<T>(topicName);
    }

    public void Unsubscribe<T>(string topicName) 
        where T : class, IMessage<T>, new()
    {
        _consumerManager.Unsubscribe<T>(topicName);
    }
}
