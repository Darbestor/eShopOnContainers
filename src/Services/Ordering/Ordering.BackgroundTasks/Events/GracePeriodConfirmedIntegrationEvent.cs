using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderGracePeriod;

namespace Ordering.BackgroundTasks.Events
{
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

    public record GracePeriodConfirmedIntegrationEvent : KafkaIntegrationEvent
    {
        public GracePeriodConfirmedIntegrationEvent(int orderId) :
            base(KafkaConstants.OrderGracePeriodTopicName, orderId.ToString(),
                new GracePeriodConfirmedProto { OrderId = orderId })
        {
        }
    }
}
