using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToCancelledIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToCancelledIntegrationEvent(int orderId, string orderStatus, string buyerName)
        : base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToCancelledProto
            {
                OrderId = orderId, OrderStatus = orderStatus, BuyerName = buyerName
            })
    {
    }
}
