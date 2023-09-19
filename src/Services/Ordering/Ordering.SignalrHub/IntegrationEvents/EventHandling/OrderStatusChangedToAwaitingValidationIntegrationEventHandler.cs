namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class
    OrderStatusChangedToAwaitingValidationIntegrationEventHandler : KafkaConsumerEventHandler<
        OrderStatusChangedToAwaitingValidationProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }


    protected override async Task HandleInternal(IMessageContext context,
        OrderStatusChangedToAwaitingValidationProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
