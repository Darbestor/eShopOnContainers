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

    public async Task Handle(ProductPriceChangedIntegrationEventProto @event)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.ProductId, @event);
    }

    public async Task Handle(IMessage @event)
    {
        var cast = (ProductPriceChangedIntegrationEventProto)@event;
        if (cast != null)
        {
            await Handle(cast);
        }
    }
}
