using Hangfire;
using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;
using Microsoft.Extensions.Options;

namespace Ordering.BackgroundTasks.IntegrationEvents.EventHandling;

public class UserCheckoutAcceptedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToSubmittedProto>
{
    private readonly GracePeriodService _gracePeriodService;
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;
    private readonly BackgroundTaskSettings _settings;

    public UserCheckoutAcceptedIntegrationEventHandler(
        ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger,
        IOptions<BackgroundTaskSettings> settings,
        GracePeriodService gracePeriodService)
        : base(logger)
    {
        _logger = logger;
        _gracePeriodService = gracePeriodService ?? throw new ArgumentNullException(nameof(gracePeriodService));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    ///     Integration event handler which schedules grace period for order
    /// </summary>
    /// <param name="context">KafkaFlow message context</param>
    /// <param name="message">
    ///     Integration event message which is sent by the
    ///     basket.api once it has successfully process the
    ///     order items.
    /// </param>
    /// <returns></returns>
    protected override Task HandleInternal(IMessageContext context, OrderStatusChangedToSubmittedProto message)
    {
        var deadline = context.ConsumerContext.MessageTimestamp.AddMinutes(_settings.GracePeriodTime);

        _logger.LogInformation("Scheduling grace period: OrderId [{OrderId}], Deadline [{Deadline}]", message.OrderId,
            deadline);


        BackgroundJob.Schedule(() => _gracePeriodService.SendIntegrationEvent(message.OrderId), deadline);
        return Task.CompletedTask;
    }
}
