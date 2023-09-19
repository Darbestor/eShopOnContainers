using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

public record KafkaOrderStockRejectedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaOrderStockRejectedIntegrationEvent(int orderId, IEnumerable<ConfirmedOrderStockItemProto> stockItems)
        : base(KafkaTopics.OrderStock, orderId.ToString(), BuildPayload(orderId, stockItems),
            Array.Empty<KeyValuePair<string, string>>()) {}

    private static OrderStockRejectedProto BuildPayload(int orderId,
        IEnumerable<ConfirmedOrderStockItemProto> stockItems)
    {
        var proto = new OrderStockRejectedProto { OrderId = orderId };
        proto.OrderStockItems.AddRange(stockItems.Select(x => new ConfirmedOrderStockItemProto()
        {
            ProductId = x.ProductId,
            HasStock = x.HasStock
        }));
        return proto;
    }
}
