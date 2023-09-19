using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.EventHandling;

public class OrderStatusChangedToSubmittedIntegrationEventHandler :
    KafkaConsumerEventHandler<OrderStatusChangedToSubmittedProto>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<OrderStatusChangedToSubmittedIntegrationEventHandler> _logger;

    public OrderStatusChangedToSubmittedIntegrationEventHandler(
        IHubContext<NotificationsHub> hubContext,
        ILogger<OrderStatusChangedToSubmittedIntegrationEventHandler> logger)
        : base(logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger;
    }


    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToSubmittedProto @event)
    {
        await _hubContext.Clients
            .Group(@event.BuyerName)
            .SendAsync("UpdatedOrderState", new { OrderId = @event.OrderId, Status = @event.OrderStatus });
    }
}
