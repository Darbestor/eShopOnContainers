using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling.Ordering;

public class OrderStatusChangedToPaidIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStatusChangedToPaidIntegrationEventProto>
{
    private readonly CatalogContext _catalogContext;
    private readonly IServiceProvider _serviceProvider;

    public OrderStatusChangedToPaidIntegrationEventHandler(
        CatalogContext catalogContext,
        ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger,
        IServiceProvider serviceProvider)
        : base(logger)
    {
        _catalogContext = catalogContext;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task HandleInternal(IMessageContext context,
        OrderStatusChangedToPaidIntegrationEventProto message)
    {
        //we're not blocking stock/inventory
        foreach (var orderStockItem in message.OrderStockItems)
        {
            var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);

            catalogItem.RemoveStock(orderStockItem.Units);
        }

        await _catalogContext.SaveChangesAsync();
    }
}
