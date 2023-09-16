using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Kafka.Producers;

public interface IEShopOnContainersProducer
{
    void Produce(KafkaIntegrationEvent @event);
}
