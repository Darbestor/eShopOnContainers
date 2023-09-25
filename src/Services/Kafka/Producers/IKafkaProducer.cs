using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Kafka.Producers;

public interface IKafkaProducer
{
    void Produce(KafkaIntegrationEvent @event);
}
