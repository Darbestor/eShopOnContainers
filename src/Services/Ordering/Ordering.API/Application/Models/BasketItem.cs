using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Basket;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Models;

public class BasketItem
{
    public string Id { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal OldUnitPrice { get; init; }
    public int Quantity { get; init; }
    public string PictureUrl { get; init; }

    public static implicit operator BasketItem(BasketItemProto itemProto) => new()
    {
        Id = itemProto.Id,
        Quantity = itemProto.Quantity,
        PictureUrl = itemProto.PictureUrl,
        ProductId = itemProto.ProductId,
        ProductName = itemProto.ProductName,
        UnitPrice = itemProto.UnitPrice,
        OldUnitPrice = itemProto.OldUnitPrice
    };
}

