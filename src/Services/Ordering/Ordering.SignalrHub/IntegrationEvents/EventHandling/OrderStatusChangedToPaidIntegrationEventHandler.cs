using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToPaidProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

    public OrderStatusChangedToPaidIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }


    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToPaidProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
