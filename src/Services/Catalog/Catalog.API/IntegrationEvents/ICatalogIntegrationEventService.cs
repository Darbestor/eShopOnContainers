namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task PublishAndSaveCatalogContextAsync(KafkaIntegrationEvent evt);
}
