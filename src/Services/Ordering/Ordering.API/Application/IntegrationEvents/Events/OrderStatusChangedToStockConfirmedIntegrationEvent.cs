using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToStockConfirmedIntegrationEvent(int orderId, string orderStatus, string buyerName)
        : base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToStockConfirmedProto
            {
                BuyerName = buyerName, OrderId = orderId, OrderStatus = orderStatus
            })
    {
    }
}
