using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderGracePeriod;

namespace Ordering.BackgroundTasks.Events;

public record GracePeriodConfirmedIntegrationEvent : KafkaIntegrationEvent
{
    public GracePeriodConfirmedIntegrationEvent(int orderId) :
        base(KafkaTopics.OrderGracePeriod, orderId.ToString(),
            new GracePeriodConfirmedProto { OrderId = orderId })
    {
    }
}
