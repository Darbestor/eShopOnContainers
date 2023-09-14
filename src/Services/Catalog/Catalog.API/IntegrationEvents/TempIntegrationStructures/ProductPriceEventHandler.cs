using Google.Protobuf;
using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;

// TODO remove
public class ProductPriceEventHandler :
    IIntegrationProtobufEventHandler<ProductPriceChangedIntegrationEventProto>
{
    private readonly ILogger<ProductPriceEventHandler> _logger;
    private readonly CatalogContext _catalogContext;

    public ProductPriceEventHandler(
        ILogger<ProductPriceEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(string key, ProductPriceChangedIntegrationEventProto message)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", key, message);
    }

    public async Task Handle(string key, IMessage message)
    {
            await Handle(key, message as ProductPriceChangedIntegrationEventProto);
    }
}

public class ProductPriceEventHandlerKafkaFlow :
    IMessageHandler<ProductPriceChangedIntegrationEventProto>
{
    private readonly ILogger<ProductPriceEventHandlerKafkaFlow> _logger;

    public ProductPriceEventHandlerKafkaFlow(ILogger<ProductPriceEventHandlerKafkaFlow> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Handle(IMessageContext context, ProductPriceChangedIntegrationEventProto message)
    {
        _logger.LogTrace("Partition: {Partition} | Offset: {Offset} | Message: {Id} | Protobuf",
            context.ConsumerContext.Partition,
            context.ConsumerContext.Offset,
            message.ProductId);

        return Task.CompletedTask;
    }
}

public class OrderStockRejectedTestHandler :
    IMessageHandler<OrderStockRejectedIntegrationEventProto>
{

    public Task Handle(IMessageContext context, OrderStockRejectedIntegrationEventProto message)
    {
        var completionSource = new TaskCompletionSource();
        Console.WriteLine(message.OrderId);
        completionSource.SetResult();
        return completionSource.Task;
    }
}
