using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog;

namespace Microsoft.eShopOnContainers.Services.Basket.API.IntegrationEvents.EventHandling.Catalog;

public class ProductPriceChangedIntegrationEventHandler : KafkaConsumerEventHandler<ProductPriceChangedProto>
{
    private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
    private readonly IBasketRepository _repository;

    public ProductPriceChangedIntegrationEventHandler(
        ILogger<ProductPriceChangedIntegrationEventHandler> logger,
        IBasketRepository repository)
    : base(logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    protected override async Task HandleInternal(IMessageContext context, ProductPriceChangedProto @event)
    {
            var userIds = _repository.GetUsers();

            foreach (var id in userIds)
            {
                var basket = await _repository.GetBasketAsync(id);

                await UpdatePriceInBasketItems(@event.ProductId, @event.NewPrice, @event.OldPrice, basket);
            }
    }

    private async Task UpdatePriceInBasketItems(int productId, decimal newPrice, decimal oldPrice, CustomerBasket basket)
    {
        var itemsToUpdate = basket?.Items?.Where(x => x.ProductId == productId).ToList();

        if (itemsToUpdate != null)
        {
            _logger.LogInformation("ProductPriceChangedIntegrationEventHandler - Updating items in basket for user: {BuyerId} ({@Items})", basket.BuyerId, itemsToUpdate);

            foreach (var item in itemsToUpdate)
            {
                if (item.UnitPrice == oldPrice)
                {
                    var originalPrice = item.UnitPrice;
                    item.UnitPrice = newPrice;
                    item.OldUnitPrice = originalPrice;
                }
            }
            await _repository.UpdateBasketAsync(basket);
        }
    }
}
