using Google.Protobuf;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

public class KafkaIntegrationEvent
{
    public string Key { get; init; }
    public IMessage Message { get; init; }
}
