using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Kafka.Consumers;

public abstract class KafkaConsumerEventHandler<T> : IMessageHandler<T>
{
    private readonly ILogger<KafkaConsumerEventHandler<T>> _logger;

    protected KafkaConsumerEventHandler(ILogger<KafkaConsumerEventHandler<T>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(IMessageContext context, T message)
    {
        if (message != null)
        {
            using (_logger.BeginScope(
                       new List<KeyValuePair<string, object>> { new("IntegrationEventContext", message) }))
            {
                _logger.LogInformation("Handling integration event: ({@IntegrationEvent})", message);

                await HandleInternal(context, message);
                context.ConsumerContext.StoreOffset();
            }
        }
    }

    protected abstract Task HandleInternal(IMessageContext context, T message);
}
