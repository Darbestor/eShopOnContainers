namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;
using Confluent.SchemaRegistry;

public class KafkaConfiguration
{
    public ProducerConfig Producer { get; set; }
    public ConsumerConfig Consumer { get; set; }
    public SchemaRegistryConfig SchemaRegistry { get; set; }
    public string BootstrapServers { get; set; }
    public string Debug { get; set; }
}
