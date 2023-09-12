using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using Microsoft.Extensions.Options;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka.Consumer;

public class KafkaProtobufConsumerBuilder<T>: IConsumerBuilder<T>
where T: class, IMessage<T>, new()
{
    private readonly ILogger<KafkaProtobufConsumerBuilder<T>> _logger;
    private readonly KafkaConfig _config;

    public KafkaProtobufConsumerBuilder(ILogger<KafkaProtobufConsumerBuilder<T>> logger, IOptions<KafkaConfig> config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }


    public IConsumer<string, T> Build()
    {
        var config = _config.Consumer;
        config.EnableAutoCommit = false;
        // TODO Change
        config.GroupId = "Test";
        return new ConsumerBuilder<string, T>(config)
            .SetValueDeserializer(new ProtobufDeserializer<T>().AsSyncOverAsync())
            .Build();
    }
}
