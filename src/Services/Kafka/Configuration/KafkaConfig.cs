using Confluent.SchemaRegistry;

namespace Microsoft.eShopOnContainers.Kafka.Configuration;

public class KafkaConfig
{
    public ProducerConfig? Producer { get; init; }

    // ReSharper disable once CollectionNeverUpdated.Global
    public Dictionary<string, ConsumerConfig> Consumers { get; init; } = new();

    public SchemaRegistryConfig SchemaRegistry { get; init; } = new();

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<string> BootstrapServers { get; init; } = new List<string>();

    public string Debug { get; init; } = "";
}
