namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

public record KafkaIntegrationEvent(string Topic, string Key, object Message,
    IEnumerable<KeyValuePair<string, string>>? Headers = null);
