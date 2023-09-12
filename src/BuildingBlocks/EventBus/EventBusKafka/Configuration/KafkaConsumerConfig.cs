namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;

public class KafkaConsumerConfig: ConsumerConfig {
    public List<string> Topics { get; set; }
}
