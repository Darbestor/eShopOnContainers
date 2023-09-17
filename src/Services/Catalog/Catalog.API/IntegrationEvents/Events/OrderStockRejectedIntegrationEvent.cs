using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

public record KafkaOrderStockRejectedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaOrderStockRejectedIntegrationEvent(int orderId, IEnumerable<ConfirmedOrderStockItemProto> stockItems)
        : base(KafkaConstants.OrderingTopicName, orderId.ToString(), BuildPayload(orderId, stockItems),
            Array.Empty<KeyValuePair<string, string>>()) {}

    private static OrderStockRejectedIntegrationEventProto BuildPayload(int orderId,
        IEnumerable<ConfirmedOrderStockItemProto> stockItems)
    {
        var proto = new OrderStockRejectedIntegrationEventProto { OrderId = orderId };
        proto.OrderStockItems.AddRange(stockItems.Select(x => new ConfirmedOrderStockItemProto()
        {
            ProductId = x.ProductId,
            HasStock = x.HasStock
        }));
        return proto;
    }
}
