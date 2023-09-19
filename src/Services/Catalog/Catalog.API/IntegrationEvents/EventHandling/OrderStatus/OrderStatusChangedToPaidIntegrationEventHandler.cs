using KafkaFlow;
using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling.OrderStatus;

public class OrderStatusChangedToPaidIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStatusChangedToPaidProto>
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
        OrderStatusChangedToPaidProto message)
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
