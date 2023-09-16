using Confluent.Kafka;

namespace Microsoft.eShopOnContainers.Kafka.Configuration;

public class KafkaProducerConfig: ProducerConfig
{
    public string Topic { get; set; }
}
