using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using Microsoft.Extensions.Hosting;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public class KafkaTopicConsumer<T>: IDisposable
    where T: class, IMessage<T>, new()
{
    private readonly ILogger<KafkaTopicConsumer<T>> _logger;
    private readonly IConsumerBuilder<T> _consumerBuilder;

    private Task _pollTask;
    private IConsumer<string, T> _consumer;
    private CancellationTokenSource _cancellationTokenSource;

    public KafkaTopicConsumer(ILogger<KafkaTopicConsumer<T>> logger, IConsumerBuilder<T> consumerBuilder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumerBuilder = consumerBuilder ?? throw new ArgumentNullException(nameof(consumerBuilder));
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
            if (_consumer == null)
            {
                _consumer = _consumerBuilder.Build();
            }
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
            while (true)
            {
                var consumeResult = _consumer.Consume(_cancellationTokenSource.Token);
                _logger.LogInformation("Message received from {TopicPartitionOffset}: {Message}",
                    consumeResult.TopicPartitionOffset, consumeResult.Message);
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

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _pollTask.Wait();
        _pollTask?.Dispose();
        _consumer?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
