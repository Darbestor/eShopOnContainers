using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}

public record KafkaOrderStockConfirmedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaOrderStockConfirmedIntegrationEvent(int orderId)
        : base(KafkaConstants.OrderingTopicName, orderId.ToString(), BuildPayload(orderId),
            Array.Empty<KeyValuePair<string, string>>()) {}

    private static OrderStockConfirmedIntegrationEventProto BuildPayload(int orderId) =>
        new OrderStockConfirmedIntegrationEventProto { OrderId = orderId };
}
