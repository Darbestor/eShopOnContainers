using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToAwaitingValidationIntegrationEvent(int orderId, string orderStatus, string buyerName,
        IEnumerable<OrderStockItemProto> orderStockItems) :
        base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToAwaitingValidationProto
            {
                OrderId = orderId,
                OrderStatus = orderStatus,
                BuyerName = buyerName,
                OrderStockItems = { orderStockItems }
            })
    { }
}

public record OrderStockItem
{
    public int ProductId { get; }
    public int Units { get; }

    public OrderStockItem(int productId, int units)
    {
        ProductId = productId;
        Units = units;
    }
}
