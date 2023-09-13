using Google.Protobuf;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;

// TODO remove
public class ProductPriceEventHandler :
    IIntegrationProtobufEventHandler<ProductPriceChangedIntegrationEventProto>
{
    private readonly ILogger<ProductPriceEventHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public ProductPriceEventHandler(
        ILogger<ProductPriceEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(string key, ProductPriceChangedIntegrationEventProto message)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", key, message);
    }

    public async Task Handle(string key, IMessage message)
    {
            await Handle(key, message as ProductPriceChangedIntegrationEventProto);
    }
}
