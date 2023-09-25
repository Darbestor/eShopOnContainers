using Microsoft.eShopOnContainers.Kafka.Producers;
using Ordering.BackgroundTasks.Events;

namespace Ordering.BackgroundTasks.Services;

public class GracePeriodService
{
    private readonly IKafkaProducer _producer;

    public GracePeriodService(IKafkaProducer producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }

    public void SendIntegrationEvent(int orderId)
    {
        _producer.Produce(new GracePeriodConfirmedIntegrationEvent(orderId));
    }
}
