using Google.Protobuf;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

// Integration Events notes: 
// An Event is “something that has happened in the past”, therefore its name has to be past tense
// An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
public record KafkaProductPriceChangedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice)
        : base(KafkaConstants.CatalogTopicName, productId.ToString(),
            new ProductPriceChangedProto { ProductId = productId, NewPrice = newPrice, OldPrice = oldPrice },
            Array.Empty<KeyValuePair<string, string>>()) {}
}
