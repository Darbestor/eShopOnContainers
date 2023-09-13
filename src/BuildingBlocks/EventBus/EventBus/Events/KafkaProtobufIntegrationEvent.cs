using Google.Protobuf;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

public class KafkaIntegrationEvent
{
    public string Topic { get; private init; }
    public string Key { get; private init; }
    public IMessage Message { get; private init; }

    public KafkaIntegrationEvent(string topic, string key, IMessage message)
    {
        Topic = topic ?? throw new ArgumentNullException(nameof(topic));
        Key = key;
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }
}
