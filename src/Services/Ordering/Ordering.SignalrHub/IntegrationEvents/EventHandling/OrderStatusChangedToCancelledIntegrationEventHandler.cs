using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class
    OrderStatusChangedToCancelledIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToCancelledProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToCancelledIntegrationEventHandler> _logger;

    public OrderStatusChangedToCancelledIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToCancelledIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }


    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToCancelledProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
