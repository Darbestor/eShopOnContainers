namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;
using Confluent.SchemaRegistry;

public class ProducerCustomConfig : ProducerConfig
{
    public string Topic { get; set; }
}

public class ConsumerCustomConfig: ConsumerConfig {
    public List<string> Topics { get; set; }
}

public class KafkaConfiguration
{
    public ProducerCustomConfig Producer { get; set; }
    public ConsumerCustomConfig Consumer { get; set; }
    public SchemaRegistryConfig SchemaRegistry { get; set; }
    public string BootstrapServers { get; set; }
    public string Debug { get; set; }
}
