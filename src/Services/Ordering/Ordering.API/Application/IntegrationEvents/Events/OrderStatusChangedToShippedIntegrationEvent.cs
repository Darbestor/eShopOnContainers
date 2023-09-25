using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToShippedIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToShippedIntegrationEvent(int orderId, string orderStatus, string buyerName)
        : base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToShippedProto
            {
                OrderId = orderId, OrderStatus = orderStatus, BuyerName = buyerName
            })
    {
    }
}
