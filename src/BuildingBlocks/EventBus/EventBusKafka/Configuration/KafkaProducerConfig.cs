namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Configuration;

public class KafkaProducerConfig : ProducerConfig
{
    public string Topic { get; set; }
}
