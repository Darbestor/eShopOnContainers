namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public interface IKafkaTopicConsumer: IDisposable
{
    void StartConsuming(string topic);
}
