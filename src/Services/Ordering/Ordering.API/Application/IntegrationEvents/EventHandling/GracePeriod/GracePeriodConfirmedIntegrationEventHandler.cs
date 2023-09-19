using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderGracePeriod;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling.GracePeriod;

public class GracePeriodConfirmedIntegrationEventHandler : KafkaConsumerEventHandler<GracePeriodConfirmedProto>
{
    private readonly IMediator _mediator;
    private readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> _logger;

    public GracePeriodConfirmedIntegrationEventHandler(
        IMediator mediator,
        ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Event handler which confirms that the grace period
    /// has been completed and order will not initially be cancelled.
    /// Therefore, the order process continues for validation. 
    /// </summary>
    /// <param name="context">KafkaFlow message context</param>
    /// <param name="event">       
    /// </param>
    /// <returns></returns>
    protected override async Task HandleInternal(IMessageContext context, GracePeriodConfirmedProto @event)
    {
        var command = new SetAwaitingValidationOrderStatusCommand(@event.OrderId);

        _logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await _mediator.Send(command);
    }
}
