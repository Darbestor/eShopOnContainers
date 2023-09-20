using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Webhooks.API.IntegrationEvents.OrderStatus;

public class OrderStatusChangedToPaidIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToPaidProto>
{
    private readonly IWebhooksRetriever _retriever;
    private readonly IWebhooksSender _sender;
    private readonly ILogger _logger;
    public OrderStatusChangedToPaidIntegrationEventHandler(IWebhooksRetriever retriever, IWebhooksSender sender, ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
    :base(logger)
    {
        _retriever = retriever;
        _sender = sender;
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToPaidProto @event)
    {
        var subscriptions = (await _retriever.GetSubscriptionsOfType(WebhookType.OrderPaid)).ToArray();
        _logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
        var whook = new WebhookData(WebhookType.OrderPaid, @event);
        await _sender.SendAll(subscriptions, whook);
    }
}
