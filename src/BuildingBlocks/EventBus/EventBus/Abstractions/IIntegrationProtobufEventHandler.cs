using Google.Protobuf;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

// TODO remove
public interface IIntegrationProtobufEventHandler<in TIntegrationEvent> : IIntegrationProtobufEventHandler
    where TIntegrationEvent : IMessage<TIntegrationEvent>
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationProtobufEventHandler
{
    Task Handle(IMessage @event);
}
