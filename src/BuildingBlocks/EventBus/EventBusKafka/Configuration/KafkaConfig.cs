namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;
using Confluent.SchemaRegistry;

public class KafkaConfig
{
    public ICollection<KafkaProducerConfig> Producers { get; set; }
    public ICollection<KafkaConsumerConfig> Consumers { get; set; }
    public SchemaRegistryConfig SchemaRegistry { get; set; }
    public ICollection<string> BootstrapServers { get; set; }
    public string Debug { get; set; }
}
