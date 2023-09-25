using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStatusChangedToStockConfirmedProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> _logger;

    public OrderStatusChangedToStockConfirmedIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }


    protected override async Task HandleInternal(IMessageContext context,
        OrderStatusChangedToStockConfirmedProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
