﻿using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

public record KafkaOrderStockConfirmedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaOrderStockConfirmedIntegrationEvent(int orderId)
        : base(KafkaConstants.OrderingTopicName, orderId.ToString(), BuildPayload(orderId),
            Array.Empty<KeyValuePair<string, string>>()) {}

    private static OrderStockConfirmedProto BuildPayload(int orderId) => new() { OrderId = orderId };
}
