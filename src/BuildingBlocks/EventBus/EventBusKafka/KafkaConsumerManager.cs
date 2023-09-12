using Google.Protobuf;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public interface IConsumerManager
{
    void Add<T>(string topic)
        where T : class, IMessage<T>, new();
    void StartConsuming();
}

public class KafkaConsumerManager : IConsumerManager
{
    private readonly ILogger<KafkaConsumerManager> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Dictionary<string, IKafkaTopicConsumer> _consumers = new();

    public KafkaConsumerManager(ILogger<KafkaConsumerManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void StartConsuming()
    {
        foreach ((string topic, IKafkaTopicConsumer consumer) in _consumers)
        {
            try
            {
                consumer.StartConsuming(topic);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error Starting consumer for topic \"{Topic}\"", topic);
            }
        }
    }

    public void Add<T>(string topic)
    where T: class, IMessage<T>, new()
    {
        if (_consumers.ContainsKey(topic))
        {
            throw new ArgumentException($"Consumer for topic '{topic}' already exist");
        }

        _logger.LogTrace("Creating Kafka consumer for topic '{Topic}'", topic);

        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<KafkaTopicConsumer<T>>>();
        var builder = scope.ServiceProvider.GetRequiredService<IConsumerBuilder<T>>();
        var consumer = new KafkaTopicConsumer<T>(logger, _serviceProvider, builder);
        
        _consumers.Add(topic, consumer);
        _logger.LogTrace("Kafka Consumer for topic '{Topic}' added", topic);
    }
}
