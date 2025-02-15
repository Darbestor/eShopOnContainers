﻿namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

[Obsolete("Old integration event used with combination of IMessageBus. KafkaIntegrationEvent should used")]
public record IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }
}
