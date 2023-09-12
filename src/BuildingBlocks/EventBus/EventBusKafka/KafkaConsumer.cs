using Microsoft.Extensions.Hosting;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public class KafkaConsumer: IDisposable
{
    private readonly KafkaConfig _config;
    private readonly ILogger<KafkaConsumer> _logger;

    private Task _pollTask;
    private IConsumer<string, string> _consumer;
    private CancellationTokenSource _cancellationTokenSource;

    public KafkaConsumer(ILogger<KafkaConsumer> logger, KafkaConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                // TODO Change
                _config.KafkaConsumer.GroupId = "Test";
            
                _consumer = new ConsumerBuilder<string, string>(_config.KafkaConsumer)
                    .Build();
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

    public void StopConsumer()
    {
        _cancellationTokenSource.Cancel();
        _pollTask.Wait();
        _pollTask = null;
        _consumer.Close();
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
        _pollTask?.Dispose();
        _consumer?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
