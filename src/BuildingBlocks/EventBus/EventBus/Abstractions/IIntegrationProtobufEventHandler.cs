using Google.Protobuf;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

// TODO remove
public interface IIntegrationProtobufEventHandler<in TIntegrationEvent> : IIntegrationProtobufEventHandler
    where TIntegrationEvent : IMessage<TIntegrationEvent>
{
    Task Handle(string key, TIntegrationEvent message);
}

public interface IIntegrationProtobufEventHandler
{
    Task Handle(string key, IMessage message);
}
