using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling.OrderStatus;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToAwaitingValidationProto>
{
    private readonly CatalogContext _catalogContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
        CatalogContext catalogContext,
        ICatalogIntegrationEventService catalogIntegrationEventService,
        ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        : base(logger)
    {
        _catalogContext = catalogContext;
        _catalogIntegrationEventService = catalogIntegrationEventService;
    }

    protected override async Task HandleInternal(IMessageContext context,
        OrderStatusChangedToAwaitingValidationProto message)
    {
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
