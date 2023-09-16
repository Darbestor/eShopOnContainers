using Confluent.Kafka;

namespace Microsoft.eShopOnContainers.Kafka.Configuration;

public class KafkaConsumerConfig: ConsumerConfig {
    public string Topic { get; set; }
}
