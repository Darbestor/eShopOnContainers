using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public class KafkaTopicConsumer<T>: IKafkaTopicConsumer
    where T: class, IMessage<T>, new()
{
    private readonly ILogger<KafkaTopicConsumer<T>> _logger;
    private readonly IServiceProvider _serviceProvider;

    private Task _pollTask;
    private readonly IConsumer<string, T> _consumer;
    private CancellationTokenSource _cancellationTokenSource;

    public KafkaTopicConsumer(ILogger<KafkaTopicConsumer<T>> logger, IServiceProvider serviceProvider, IConsumerBuilder<T> consumerBuilder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _consumer = consumerBuilder.Build();
    }
    
    public void StartConsuming(string topic)
    {
        if (_pollTask != null)
        {
            throw new ApplicationException($"Kafka Consumer for topic '{topic}' already running");
        }
        
        _logger.LogTrace("Starting Kafka topic '{Topic}' consumer", topic);
        
        try
        {
            _consumer.Subscribe(topic);

            _cancellationTokenSource = new CancellationTokenSource();
            _pollTask = Task.Run(async () => await StartPollLoop(), _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Starting consumer for topic \"{Topic}\"", topic);
        }
    }

    private async Task StartPollLoop()
    {
        try
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                ConsumeResult<string, T> consumeResult = _consumer.Consume(_cancellationTokenSource.Token);
                _logger.LogInformation("Message received from {TopicPartitionOffset}: {Id}",
                    consumeResult.TopicPartitionOffset, consumeResult.Message.Key);
                await ConsumerReceived(consumeResult);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogError("Error processing consumer {Consumer}", _consumer.Name);
            // The consumer was stopped via cancellation token.
        }
        finally
        {
            _logger.LogInformation("{Consumer} finished job", _consumer.Name);

            _consumer.Close();
        }
    }
    
    private async Task ConsumerReceived(ConsumeResult<string, T> consumeResult)
    {
        try
        {
            await Task.Yield();
            await ProcessEvent(consumeResult.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error Processing message \"{Message}\"", consumeResult.Message.Key);
        }

        _consumer.Commit(consumeResult);
    }

    private async Task ProcessEvent(Message<string, T> message)
    {
        var genericType = message.Value.GetGenericTypeName();

        _logger.LogTrace("Processing Kafka event: {EventName}", genericType);
        
        await using var scope = _serviceProvider.CreateAsyncScope();
        var type = typeof(IIntegrationProtobufEventHandler<>).MakeGenericType(message.Value.GetType());
        var handler = (IIntegrationProtobufEventHandler)scope.ServiceProvider.GetService(type);
        if (handler == null)
        {
            _logger.LogTrace("Event {EventName} don't have handler", genericType);
            return;
        }
        await handler.Handle(message.Value);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _pollTask.Wait();
        _pollTask?.Dispose();
        _consumer?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
