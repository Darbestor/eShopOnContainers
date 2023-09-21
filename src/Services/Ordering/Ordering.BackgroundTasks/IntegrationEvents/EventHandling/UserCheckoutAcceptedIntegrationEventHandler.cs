using Hangfire;
using KafkaFlow;
using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Kafka.Producers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;
using Ordering.BackgroundTasks.Events;

namespace Ordering.BackgroundTasks.IntegrationEvents.EventHandling;

public class UserCheckoutAcceptedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToSubmittedProto>
{
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;
    private readonly IKafkaProducer _producer;

    public UserCheckoutAcceptedIntegrationEventHandler(
        ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger,
        IKafkaProducer producer)
    :base(logger)
    {
        _logger = logger;
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }

    /// <summary>
    /// Integration event handler which starts the create order process
    /// </summary>
    /// <param name="context">KafkaFlow message context</param>
    /// <param name="message">
    /// Integration event message which is sent by the
    /// basket.api once it has successfully process the 
    /// order items.</param>
    /// <returns></returns>
    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToSubmittedProto message)
    {
        BackgroundJob.Schedule(() => _producer.Produce(new GracePeriodConfirmedIntegrationEvent(message.OrderId)),
            TimeSpan.FromMinutes(1));
    }
}
