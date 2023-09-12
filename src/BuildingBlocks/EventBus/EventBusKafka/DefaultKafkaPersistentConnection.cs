namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public class DefaultKafkaPersistentConnection
    : IKafkaPersistentConnection
{
    private readonly KafkaConfig _config;
    private readonly ILogger<DefaultKafkaPersistentConnection> _logger;
    private readonly int _retryCount;
    public bool Disposed;

    readonly object _syncRoot = new();
    private readonly IProducer<byte[], byte[]> _producer;

    public DefaultKafkaPersistentConnection(KafkaConfig config, ILogger<DefaultKafkaPersistentConnection> logger, int retryCount = 5)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = retryCount;
        try
        {
            _producer = new ProducerBuilder<byte[], byte[]>(_config.KafkaProducer)
                .SetErrorHandler(OnErrorHandle)
                .Build();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Handle Handle { get => _producer.Handle; }
    public KafkaConfig KafkaConfig { get => _config; }

    public void Dispose()
    {
        if (Disposed) return;

        Disposed = true;

        try
        {
            _producer.Flush();
            _producer.Dispose();
        }
        catch (IOException ex)
        {
            _logger.LogCritical(ex.ToString());
        }
    }
    
    private void OnErrorHandle(IProducer<byte[], byte[]> producer, Error error)
    {
        if (Disposed) return;
        _logger.LogError("A Kafka error " + error);
    }
}
