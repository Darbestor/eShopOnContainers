namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;
using Confluent.SchemaRegistry;

public class KafkaConfig
{
    public KafkaProducerConfig KafkaProducer { get; set; }
    public KafkaConsumerConfig KafkaConsumer { get; set; }
    public SchemaRegistryConfig SchemaRegistry { get; set; }
    public string BootstrapServers { get; set; }
    public string Debug { get; set; }
}
