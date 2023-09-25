using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToPaidIntegrationEvent(int orderId,
        string orderStatus,
        string buyerName,
        IEnumerable<OrderStockItemProto> orderStockItems)
        : base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToPaidProto
            {
                OrderId = orderId,
                OrderStatus = orderStatus,
                BuyerName = buyerName,
                OrderStockItems = { orderStockItems }
            })
    {
    }
}
