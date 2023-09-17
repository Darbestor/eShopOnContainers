using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.TempIntegrationStructures;

// TODO REmove
public class ProductPriceEventHandlerKafkaFlow :
    IMessageHandler<ProductPriceChangedProtobuf>
{
    private readonly ILogger<ProductPriceEventHandlerKafkaFlow> _logger;

    public ProductPriceEventHandlerKafkaFlow(ILogger<ProductPriceEventHandlerKafkaFlow> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Handle(IMessageContext context, ProductPriceChangedProtobuf message)
    {
        _logger.LogTrace("Partition: {Partition} | Offset: {Offset} | Message: {Id} | Protobuf",
            context.ConsumerContext.Partition,
            context.ConsumerContext.Offset,
            message.ProductId);

        return Task.CompletedTask;
    }
}
