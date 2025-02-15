﻿using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Webhooks.API.IntegrationEvents.OrderStatus;

public class OrderStatusChangedToShippedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStatusChangedToShippedProto>
{
    private readonly IWebhooksRetriever _retriever;
    private readonly IWebhooksSender _sender;
    private readonly ILogger _logger;
    public OrderStatusChangedToShippedIntegrationEventHandler(IWebhooksRetriever retriever, IWebhooksSender sender, ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger)
    :base(logger)
    {
        _retriever = retriever;
        _sender = sender;
        _logger = logger;
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStatusChangedToShippedProto @event)
    {
        var subscriptions = (await _retriever.GetSubscriptionsOfType(WebhookType.OrderShipped)).ToArray();
        _logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());
        var whook = new WebhookData(WebhookType.OrderShipped, @event);
        await _sender.SendAll(subscriptions, whook);
    }
}
