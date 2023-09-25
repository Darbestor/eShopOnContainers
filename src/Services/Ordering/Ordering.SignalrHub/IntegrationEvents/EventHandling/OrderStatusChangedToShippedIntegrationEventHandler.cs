namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class
    OrderStatusChangedToShippedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToShippedProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToShippedIntegrationEventHandler> _logger;

    public OrderStatusChangedToShippedIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToShippedProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
