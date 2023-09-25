using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;

namespace Microsoft.eShopOnContainers.Payment.API.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent : KafkaIntegrationEvent
{
    public OrderPaymentSucceededIntegrationEvent(int orderId) :
        base(KafkaTopics.OrderPayment, orderId.ToString(), new OrderPaymentSucceededProto { OrderId = orderId })
    {
    }
}
