using Google.Protobuf;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler :
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    private readonly CatalogContext _catalogContext;
    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OrderStatusChangedToPaidIntegrationEventHandler(
        CatalogContext catalogContext,
        ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger,
        IServiceProvider serviceProvider)
    {
        _catalogContext = catalogContext;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new ("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            //we're not blocking stock/inventory
            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);

                catalogItem.RemoveStock(orderStockItem.Units);
            }

            await _catalogContext.SaveChangesAsync();

        }
    }
}
