namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public interface IConsumerBuilder<T>
{
    IConsumer<string, T> Build();
}
