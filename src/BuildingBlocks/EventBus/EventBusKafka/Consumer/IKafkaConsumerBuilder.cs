namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public interface IKafkaConsumerBuilder<T>
{
    IConsumer<string, T> Build();
}
