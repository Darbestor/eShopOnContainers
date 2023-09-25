namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

/// <summary>
/// Integration event containing message
/// and required metadata for Kafka system
/// </summary>
/// <param name="Topic">Kafka topic where a message is produced</param>
/// <param name="Key">Event key</param>
/// <param name="Message">Payload</param>
/// <param name="Headers">Additional metadata</param>
public record KafkaIntegrationEvent(string Topic, string Key, object Message,
    IEnumerable<KeyValuePair<string, string>>? Headers = null);
