using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Basket;

namespace Microsoft.eShopOnContainers.Services.Basket.API.Model;

public class CustomerBasket
{
    public string BuyerId { get; set; }

    public List<BasketItem> Items { get; set; } = new();

    public CustomerBasket()
    {

    }

    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
    }
    
    public static implicit operator CustomerBasketProto(CustomerBasket basket)
    {
        var protoBasket = new CustomerBasketProto { BuyerId = basket.BuyerId, };
        protoBasket.Items.AddRange(basket.Items.ConvertAll(x => (BasketItemProto)x));
        return protoBasket;
    }
}

