namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public class DefaultKafkaPersistentConnection
    : IKafkaPersistentConnection
{
    private readonly ClientConfig _config;
    private readonly ILogger<DefaultKafkaPersistentConnection> _logger;
    private readonly int _retryCount;
    public bool Disposed;

    readonly object _syncRoot = new();
    private readonly IProducer<byte[], byte[]> _producer;

    public DefaultKafkaPersistentConnection(ClientConfig config, ILogger<DefaultKafkaPersistentConnection> logger, int retryCount = 5)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = retryCount;
        _producer = new ProducerBuilder<byte[], byte[]>(_config)
            .SetErrorHandler(OnErrorHandle)
            .SetLogHandler(OnLogHandler)
            .Build();
    }

    public Handle Handle { get => _producer.Handle; }

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

    private void OnLogHandler(IProducer<byte[], byte[]> producer, LogMessage log)
    {
        if (Disposed) return;
            var logLevel = log.Level switch
            {
                SyslogLevel.Critical or SyslogLevel.Alert or SyslogLevel.Emergency => LogLevel.Critical,
                SyslogLevel.Error => LogLevel.Error,
                SyslogLevel.Warning => LogLevel.Warning,
                SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
                SyslogLevel.Debug => LogLevel.Debug,
                _ => throw new ArgumentOutOfRangeException()
            };
            _logger.Log(logLevel, "A Kafka client {Name} reports: {Message}", log.Name, log.Message);
    }
    
    private void OnErrorHandle(IProducer<byte[], byte[]> producer, Error error)
    {
        if (Disposed) return;
        _logger.LogError("A Kafka error " + error);
    }
}
