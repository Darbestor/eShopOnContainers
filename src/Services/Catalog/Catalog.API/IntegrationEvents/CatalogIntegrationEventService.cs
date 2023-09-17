using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.eShopOnContainers.Kafka.Producers;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents;

public class CatalogIntegrationEventService : ICatalogIntegrationEventService
{
    private readonly IEShopOnContainersProducer _kafkaProducer;
    private readonly CatalogContext _catalogContext;
    private readonly ILogger<CatalogIntegrationEventService> _logger;

    public CatalogIntegrationEventService(
        ILogger<CatalogIntegrationEventService> logger,
        IEShopOnContainersProducer kafkaProducer,
        CatalogContext catalogContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
    }

    public async Task PublishAndSaveCatalogContextAsync(KafkaIntegrationEvent evt)
    {
        try
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Topic, evt.Message.GetGenericTypeName());

            _kafkaProducer.Produce(evt);
            await _catalogContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Topic, evt.Message.GetGenericTypeName());
        }
    }
}
