namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Producer;


public interface IKafkaProtobufProducer<T>: IKafkaProtobufProducer
    where T: class, IMessage<T>, new()
{ }

public interface IKafkaProtobufProducer
{
    void Produce(string topic, KafkaIntegrationEvent @event);
}

