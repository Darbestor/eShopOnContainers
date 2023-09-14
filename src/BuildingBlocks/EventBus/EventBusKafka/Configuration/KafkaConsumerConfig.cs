namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;

public class KafkaConsumerConfig: ConsumerConfig {
    public string Topic { get; set; }
}
