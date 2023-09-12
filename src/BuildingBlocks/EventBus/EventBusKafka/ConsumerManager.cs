using Microsoft.Extensions.Options;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

internal class ConsumerPoll
{
    public string TopicName { get; }
    public Task PollTask { get; }
    public CancellationTokenSource CancellationTokenSource { get; }
    
    public ConsumerPoll(string topicName, Task pollTask, CancellationTokenSource cancellationTokenSource)
    {
        TopicName = topicName; 
        PollTask = pollTask;
        CancellationTokenSource = cancellationTokenSource;
    }
}

public interface IConsumerManager
{
    void StartConsuming(string topicName);
}

public class ConsumerManager : IConsumerManager
{
    private readonly ILogger<ConsumerManager> _logger;
    private readonly KafkaConfig _config;
    private List<ConsumerPoll> _pollLoops = new();
    private readonly TaskFactory _taskFactory;

    public ConsumerManager(ILogger<ConsumerManager> logger, IOptions<KafkaConfig> config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config.Value;
        _taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
    }
    
    public void StartConsuming(string topicName)
    {
        if (_pollLoops.Exists(x => x.TopicName == topicName)) return;
        
        _logger.LogTrace("Starting Kafka topic '{Topic}' consume", topicName);
        
        try
        {
            _config.Consumer.GroupId = "Test";
            var consumer = new ConsumerBuilder<string, string>(_config.Consumer)
                .Build();
            consumer.Subscribe(topicName);

            var cancellationTokenSource = new CancellationTokenSource();
            var task = _taskFactory.StartNew(() => StartPollLoop(consumer, cancellationTokenSource.Token), cancellationTokenSource.Token);
            var consumerPoll = new ConsumerPoll(topicName, task, cancellationTokenSource);
            _pollLoops.Add(consumerPoll);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error Starting consumer for topic \"{Topic}\"", topicName);
        }
    }

    private void StartPollLoop(IConsumer<string, string> consumer, CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                _logger.LogInformation("Message received from {TopicPartitionOffset}: {Message}",
                    consumeResult.TopicPartitionOffset, consumeResult.Message);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogError("Error processing consumer {Consumer}", consumer.Name);
            // The consumer was stopped via cancellation token.
        }
        finally
        {
            _logger.LogInformation("{Consumer} finished job", consumer.Name);

            consumer.Close();
        }
    }
}
