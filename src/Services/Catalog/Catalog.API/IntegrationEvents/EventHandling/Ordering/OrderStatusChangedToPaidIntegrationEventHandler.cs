using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling.Ordering;

public class OrderStatusChangedToPaidIntegrationEventHandler :
    IMessageHandler<OrderStatusChangedToPaidIntegrationEventProto>
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

    public async Task Handle(IMessageContext context, OrderStatusChangedToPaidIntegrationEventProto message)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new ("IntegrationEventContext", message.OrderId) }))
        {
            _logger.LogInformation("Handling integration event: ({@IntegrationEvent})", message);

            //we're not blocking stock/inventory
            foreach (var orderStockItem in message.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);

                catalogItem.RemoveStock(orderStockItem.Units);
            }

            await _catalogContext.SaveChangesAsync();
        }
    }
}
