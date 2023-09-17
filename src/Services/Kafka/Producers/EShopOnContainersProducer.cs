using System.Text;
using Confluent.Kafka;
using KafkaFlow;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Kafka.Producers;

public class EShopOnContainersProducer : IEShopOnContainersProducer
{
    private readonly ILogger<EShopOnContainersProducer> _logger;
    private readonly IMessageProducer<EShopOnContainersProducer> _kafkaflowProducer;

    public EShopOnContainersProducer(ILogger<EShopOnContainersProducer> logger, IMessageProducer<EShopOnContainersProducer> kafkaflowProducer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _kafkaflowProducer = kafkaflowProducer ?? throw new ArgumentNullException(nameof(kafkaflowProducer));
    }

    public void Produce(KafkaIntegrationEvent @event)
    {
        var (topic, key, message, headers) = @event;
        _logger.LogTrace(@"Producing integration event: {Event}", @event);

        var kafkaHeaders = new MessageHeaders();
        foreach (var (headerKey, headerValue) in headers)
        {
            kafkaHeaders.Add(headerKey, Encoding.ASCII.GetBytes(headerValue));
        }
        
        _kafkaflowProducer.Produce(topic, key, message, kafkaHeaders);
    }
}
