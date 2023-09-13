using Google.Protobuf;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public class KafkaEventBus : IKafkaEventBus
{
    private readonly IKafkaPersistentConnection _persistentConnection;
    private readonly ILogger<KafkaEventBus> _logger;
    private readonly IConsumerManager _consumerManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly int _retryCount;
    
    public KafkaEventBus(IKafkaPersistentConnection persistentConnection, ILogger<KafkaEventBus> logger,
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


    public void Publish(KafkaIntegrationEvent @event)
    {
        var eventType = @event.Message.Descriptor.ClrType;
        using var scope = _serviceProvider.CreateScope();
        var type = typeof(IKafkaProtobufProducer<>).MakeGenericType(eventType);
        if (scope.ServiceProvider.GetService(type) is not IKafkaProtobufProducer producer)
        {
            _logger.LogError("Kafka Producer for {EventName} not registered", @event.Message.GetGenericTypeName());
            return;
        }
        
        producer.Produce(@event);
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
