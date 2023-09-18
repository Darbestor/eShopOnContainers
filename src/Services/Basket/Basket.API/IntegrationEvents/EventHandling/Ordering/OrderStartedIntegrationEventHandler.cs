using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Basket.API.IntegrationEvents.EventHandling.Ordering;

public class OrderStartedIntegrationEventHandler : KafkaConsumerEventHandler<OrderStartedIntegrationEventProto>
{
    private readonly IBasketRepository _repository;

    public OrderStartedIntegrationEventHandler(
        IBasketRepository repository,
        ILogger<OrderStartedIntegrationEventHandler> logger):
        base(logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    protected override async Task HandleInternal(IMessageContext context, OrderStartedIntegrationEventProto message)
    {
        await _repository.DeleteBasketAsync(message.UserId.ToString());
    }
}




