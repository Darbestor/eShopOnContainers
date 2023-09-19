using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Kafka.Producers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Payment.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStatusChangedToStockConfirmedProto>
{
    private readonly PaymentSettings _settings;
    private readonly IEShopOnContainersProducer _producer;
    private readonly ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> _logger;

    public OrderStatusChangedToStockConfirmedIntegrationEventHandler(
        IEShopOnContainersProducer producer,
        IOptionsSnapshot<PaymentSettings> settings,
        ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        _settings = settings.Value;
        _producer = producer;
        _logger = logger;

        _logger.LogTrace("PaymentSettings: {@PaymentSettings}", _settings);
    }

    protected override async Task HandleInternal(IMessageContext context,
        OrderStatusChangedToStockConfirmedProto @event)
    {
        KafkaIntegrationEvent orderPaymentIntegrationEvent;

        //Business feature comment:
        // When OrderStatusChangedToStockConfirmed Integration Event is handled.
        // Here we're simulating that we'd be performing the payment against any payment gateway
        // Instead of a real payment we just take the env. var to simulate the payment 
        // The payment can be successful or it can fail

        if (_settings.PaymentSucceeded)
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(@event.OrderId);
        }
        else
        {
            orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
        }

        _producer.Produce(orderPaymentIntegrationEvent);

        await Task.CompletedTask;
    }
}
