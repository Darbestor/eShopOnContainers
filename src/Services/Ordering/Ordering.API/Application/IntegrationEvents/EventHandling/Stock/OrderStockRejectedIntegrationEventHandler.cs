using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderPayment;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling.Stock;

public class OrderStockRejectedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStockRejectedProto>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStockRejectedIntegrationEventHandler> _logger;

    public OrderStockRejectedIntegrationEventHandler(
        IMediator mediator,
        ILogger<OrderStockRejectedIntegrationEventHandler> logger)
        : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStockRejectedProto @event)
    {
        var orderStockRejectedItems = @event.OrderStockItems
            .Where(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        var command = new SetStockRejectedOrderStatusCommand(@event.OrderId, orderStockRejectedItems);

        _logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await _mediator.Send(command);
    }
}
