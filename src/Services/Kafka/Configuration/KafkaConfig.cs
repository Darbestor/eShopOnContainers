using Confluent.SchemaRegistry;

namespace Microsoft.eShopOnContainers.Kafka.Configuration;

public class SchemaRegistryConf: SchemaRegistryConfig
{
    public string Url { get; set; }
}

public class KafkaConfig
{
    public Dictionary<string, KafkaProducerConfig> Producers { get; set; }
    public Dictionary<string, KafkaConsumerConfig> Consumers { get; set; }
    public SchemaRegistryConf SchemaRegistry { get; set; }
    public ICollection<string> BootstrapServers { get; set; }
    public string Debug { get; set; }
}
