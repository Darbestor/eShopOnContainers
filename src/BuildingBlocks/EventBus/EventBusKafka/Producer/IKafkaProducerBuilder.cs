namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;

public interface IKafkaProducerBuilder<T>
{
    IProducer<string, T> Build();
}
