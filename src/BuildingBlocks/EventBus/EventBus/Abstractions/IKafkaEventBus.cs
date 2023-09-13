using Google.Protobuf;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

//TODO Remove
public interface IKafkaEventBus
{
    void Publish(string key, KafkaIntegrationEvent @event);
    void Subscribe<T>(string topicName)
        where T : class, IMessage<T>, new();

    public void Unsubscribe<T>(string topicName)
        where T : class, IMessage<T>, new();
}
