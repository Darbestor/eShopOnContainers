using System.Text;
using Confluent.Kafka;
using KafkaFlow;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Kafka.Producers;

public class KafkaProducer : IKafkaProducer
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IMessageProducer<KafkaProducer> _kafkaflowProducer;

    public KafkaProducer(ILogger<KafkaProducer> logger, IMessageProducer<KafkaProducer> kafkaflowProducer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _kafkaflowProducer = kafkaflowProducer ?? throw new ArgumentNullException(nameof(kafkaflowProducer));
    }

    public void Produce(KafkaIntegrationEvent @event)
    {
        var (topic, key, message, headers) = @event;
        _logger.LogInformation(@"Producing integration event: {Event}", @event);

        var kafkaHeaders = new MessageHeaders();
        if (headers != null)
        {
            foreach (var (headerKey, headerValue) in headers)
            {
                kafkaHeaders.Add(headerKey, Encoding.ASCII.GetBytes(headerValue));
            }
        }

        _kafkaflowProducer.Produce(topic, key, message, kafkaHeaders);
    }
}
