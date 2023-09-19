using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling.Payment;

public class OrderPaymentFailedIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderPaymentFailedProto>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;

    public OrderPaymentFailedIntegrationEventHandler(
        IMediator mediator,
        ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
        : base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderPaymentFailedProto @event)
    {
        var command = new CancelOrderCommand(@event.OrderId);

        _logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await _mediator.Send(command);
    }
}
