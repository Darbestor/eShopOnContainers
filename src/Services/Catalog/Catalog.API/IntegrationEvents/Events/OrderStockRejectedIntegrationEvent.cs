using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public List<ConfirmedOrderStockItem> OrderStockItems { get; }

    public OrderStockRejectedIntegrationEvent(int orderId,
        List<ConfirmedOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

public record ConfirmedOrderStockItem
{
    public int ProductId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(int productId, bool hasStock)
    {
        ProductId = productId;
        HasStock = hasStock;
    }
}

public record KafkaOrderStockRejectedIntegrationEvent : KafkaIntegrationEvent
{
    public KafkaOrderStockRejectedIntegrationEvent(int orderId, IEnumerable<ConfirmedOrderStockItem> stockItems)
        : base(KafkaConstants.OrderingTopicName, orderId.ToString(), BuildPayload(orderId, stockItems),
            Array.Empty<KeyValuePair<string, string>>()) {}

    private static OrderStockRejectedIntegrationEventProto BuildPayload(int orderId,
        IEnumerable<ConfirmedOrderStockItem> stockItems)
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
