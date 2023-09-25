using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;

namespace Microsoft.eShopOnContainers.Payment.API.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent : KafkaIntegrationEvent
{
    public OrderPaymentFailedIntegrationEvent(int orderId) :
        base(KafkaTopics.OrderPayment, orderId.ToString(), new OrderPaymentFailedProto { OrderId = orderId })
    {
    }
}
