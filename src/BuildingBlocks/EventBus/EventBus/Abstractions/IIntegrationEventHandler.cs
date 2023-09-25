namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

[Obsolete]
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler
{
}
