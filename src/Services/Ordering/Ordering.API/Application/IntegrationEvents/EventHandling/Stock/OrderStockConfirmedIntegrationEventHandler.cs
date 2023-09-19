using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStock;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling.Stock;

public class OrderStockConfirmedIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStockConfirmedProto>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStockConfirmedIntegrationEventHandler> _logger;

    public OrderStockConfirmedIntegrationEventHandler(
        IMediator mediator,
        ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStockConfirmedProto @event)
    {
        var command = new SetStockConfirmedOrderStatusCommand(@event.OrderId);

        _logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await _mediator.Send(command);
    }
}
