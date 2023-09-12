namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;
using Confluent.SchemaRegistry;

public class KafkaConfig
{
    public KafkaProducerConfig Producer { get; set; }
    public KafkaConsumerConfig Consumer { get; set; }
    public SchemaRegistryConfig SchemaRegistry { get; set; }
    public string BootstrapServers { get; set; }
    public string Debug { get; set; }
}
