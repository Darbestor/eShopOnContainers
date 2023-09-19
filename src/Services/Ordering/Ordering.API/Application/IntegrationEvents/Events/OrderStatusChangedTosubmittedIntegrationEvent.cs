using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToSubmittedIntegrationEvent : KafkaIntegrationEvent
{
    public OrderStatusChangedToSubmittedIntegrationEvent(int orderId, string orderStatus, string buyerName)
        : base(KafkaConstants.OrderStatusTopicName, orderId.ToString(),
            new OrderStatusChangedToSubmittedProto
            {
                OrderId = orderId, OrderStatus = orderStatus, BuyerName = buyerName
            })
    {
    }
}
