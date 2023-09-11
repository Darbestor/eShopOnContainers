using Microsoft.Extensions.Hosting;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

public class KafkaConsumer: BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(ConsumerConfig config, ILogger<KafkaConsumer> logger)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
