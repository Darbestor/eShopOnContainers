using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling.Ordering;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
    IMessageHandler<OrderStatusChangedToAwaitingValidationIntegrationEventProto>
{
    private readonly CatalogContext _catalogContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
        CatalogContext catalogContext,
        ICatalogIntegrationEventService catalogIntegrationEventService,
        ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
    {
        _catalogContext = catalogContext;
        _catalogIntegrationEventService = catalogIntegrationEventService;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Handle(IMessageContext context, OrderStatusChangedToAwaitingValidationIntegrationEventProto message)
    {
        using (_logger.BeginScope(
                   new List<KeyValuePair<string, object>> { new("IntegrationEventContext", message.OrderId) }))
        {
            _logger.LogInformation("Handling integration event: ({@IntegrationEvent})", message);

            var confirmedOrderStockItems = new List<ConfirmedOrderStockItemProto>();

            foreach (var orderStockItem in message.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);
                var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                var confirmedOrderStockItem =
                    new ConfirmedOrderStockItemProto { ProductId = catalogItem.Id, HasStock = hasStock };

                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }

            KafkaIntegrationEvent confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                ? new KafkaOrderStockRejectedIntegrationEvent(message.OrderId, confirmedOrderStockItems)
                : new KafkaOrderStockConfirmedIntegrationEvent(message.OrderId);

            await _catalogIntegrationEventService.PublishAndSaveCatalogContextAsync(confirmedIntegrationEvent);
        }
    }
}
