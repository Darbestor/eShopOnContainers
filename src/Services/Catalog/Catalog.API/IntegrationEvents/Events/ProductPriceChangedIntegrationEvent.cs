using Google.Protobuf;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

// Integration Events notes: 
// An Event is “something that has happened in the past”, therefore its name has to be past tense
// An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
public record ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; private init; }

    public decimal NewPrice { get; private init; }

    public decimal OldPrice { get; private init; }

    public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
        OldPrice = oldPrice;
    }
}

public interface IKafkaEvent
{
    string Key { get; }
    object MessageRaw { get; }
}

public abstract class KafkaEvent<T>: IKafkaEvent
    where T : class, IMessage<T>, new()
{
    public string Key { get; private init; }
    public object MessageRaw => Message;
    public IMessage<T> Message { get; private init; }

    protected KafkaEvent(string key, IMessage<T> message)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }
    
    public void Deconstruct(
        out object messageKey,
        out object messageValue)
    {
        (messageKey, messageValue) = (Key, Message);
    }
}

// public class ProductPriceChangedIntegrationEventKafka: KafkaEvent<ProductPriceChangedIntegrationEventProto>
// {
//     public ProductPriceChangedIntegrationEventKafka(string key, ProductPriceChangedIntegrationEventProto message)
//         : base(key, message)
//     {}
// }
//
// public class OrderStockRejectedIntegrationEventKafka: KafkaEvent<OrderStockRejectedIntegrationEventProto>
// {
//     public OrderStockRejectedIntegrationEventKafka(string key, IMessage<OrderStockRejectedIntegrationEventProto> message) : base(key, message)
//     { }
// }
