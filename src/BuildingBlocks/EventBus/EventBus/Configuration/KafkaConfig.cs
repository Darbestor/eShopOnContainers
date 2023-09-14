namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Configuration;

public class SchemaRegistryConf
{
    public string Url { get; set; }
}

public class KafkaConfig
{
    public ICollection<KafkaProducerConfig> Producers { get; set; }
    public ICollection<KafkaConsumerConfig> Consumers { get; set; }
    public SchemaRegistryConf SchemaRegistry { get; set; }
    public ICollection<string> BootstrapServers { get; set; }
    public string Debug { get; set; }
}
