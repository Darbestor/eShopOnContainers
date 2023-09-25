namespace Microsoft.eShopOnContainers.Services.Ordering.API;

public static class KafkaConstants
{
    public const string OrderingTopicName = "Ordering";
    public const string BasketTopicName = "Basket";
    public const string OrderStatusTopicName = "OrderStatus";
    public const string OrderPaymentTopicName = "OrderPayment";
    public const string OrderStockTopicName = "OrderStock";
    public const string OrderGracePeriodTopicName = "OrderGracePeriod";
}
