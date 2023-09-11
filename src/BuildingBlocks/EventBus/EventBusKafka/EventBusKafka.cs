namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

// TODO REMOVE
public interface IEventBusTemp {}

public class EventBusKafka : IEventBusTemp, IDisposable
{
    private readonly IKafkaPersistentConnection _persistentConnection;
    private readonly ILogger<EventBusKafka> _logger;
    private readonly IConsumerManager _consumerManager;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly int _retryCount;

    private Dictionary<string, List<string>> _topicSubscriptions = new();
    
    public EventBusKafka(IKafkaPersistentConnection persistentConnection, ILogger<EventBusKafka> logger,
        IConsumerManager consumerManager, IServiceProvider serviceProvider,
        IEventBusSubscriptionsManager subsManager, int retryCount = 5)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumerManager = consumerManager ?? throw new ArgumentNullException(nameof(consumerManager));
        _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
        _serviceProvider = serviceProvider;
        _retryCount = retryCount;
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    private void SubsManager_OnEventRemoved(object sender, string eventName)
    {
        // if (!_persistentConnection.IsConnected)
        // {
        //     _persistentConnection.TryConnect();
        // }
        //
        // using var channel = _persistentConnection.CreateModel();
        // channel.QueueUnbind(queue: _queueName,
        //     exchange: BROKER_NAME,
        //     routingKey: eventName);
        //
        // if (_subsManager.IsEmpty)
        // {
        //     _queueName = string.Empty;
        //     _consumerChannel.Close();
        // }
    }

    public void Publish(IntegrationEvent @event)
    {
        using var producer = new DependentProducerBuilder<string, string>(_persistentConnection.Handle)
            .Build();
        
        var policy = RetryPolicy.Handle<ProduceException<string, string>>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s", @event.Id, $"{time.TotalSeconds:n1}");
            });

        var eventName = @event.GetType().Name;

        var serialized = JsonSerializer.Serialize(@event);
        
        var message = new Message<string, string> { Key = eventName, Value = serialized };

        policy.Execute(() =>
        {
            _logger.LogTrace("Publishing event to Kafka: {EventId}", @event.Id);

            producer.Produce(_persistentConnection.KafkaConfig.Producer.Topic, message);
        });
    }

    public void Subscribe<T, TH>(string topicName)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();
        DoInternalSubscription(eventName, topicName);

        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

        // _subsManager.AddSubscription<T, TH>();
        StartBasicConsume(topicName);
    }

    private void DoInternalSubscription(string eventName, string topicName)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (_topicSubscriptions.TryGetValue(topicName, out var list))
            {
                list.Add(eventName);
                return;
            }
            _topicSubscriptions.Add(topicName, new List<string>{eventName});
            _consumerManager.StartConsuming(topicName);
        }
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    public void Dispose()
    {
        _subsManager.Clear();
    }

    private void StartBasicConsume(string topicName)
    {
        // _logger.LogTrace("Starting RabbitMQ basic consume");
        //
        // if (_consumerChannel != null)
        // {
        //     var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
        //
        //     consumer.Received += Consumer_Received;
        //
        //     _consumerChannel.BasicConsume(
        //         queue: _queueName,
        //         autoAck: false,
        //         consumer: consumer);
        // }
        // else
        // {
        //     _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
        // }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        // _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        //
        // if (_subsManager.HasSubscriptionsForEvent(eventName))
        // {
        //     await using var scope = _serviceProvider.CreateAsyncScope();
        //     var subscriptions = _subsManager.GetHandlersForEvent(eventName);
        //     foreach (var subscription in subscriptions)
        //     {
        //         if (subscription.IsDynamic)
        //         {
        //             if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
        //             using dynamic eventData = JsonDocument.Parse(message);
        //             await Task.Yield();
        //             await handler.Handle(eventData);
        //         }
        //         else
        //         {
        //             var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
        //             if (handler == null) continue;
        //             var eventType = _subsManager.GetEventTypeByName(eventName);
        //             var integrationEvent = JsonSerializer.Deserialize(message, eventType, s_caseInsensitiveOptions);
        //             var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
        //
        //             await Task.Yield();
        //             await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
        //         }
        //     }
        // }
        // else
        // {
        //     _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
        // }
    }
}
