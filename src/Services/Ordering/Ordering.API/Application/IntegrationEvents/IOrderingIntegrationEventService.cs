namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents;

public interface IOrderingIntegrationEventService
{
    void PublishEvent(KafkaIntegrationEvent evt);
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}
