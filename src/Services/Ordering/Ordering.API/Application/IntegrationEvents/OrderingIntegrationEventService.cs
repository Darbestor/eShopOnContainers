using Microsoft.eShopOnContainers.Kafka.Producers;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents;

public class OrderingIntegrationEventService : IOrderingIntegrationEventService
{
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly OrderingContext _orderingContext;
    private readonly IKafkaProducer _producer;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<OrderingIntegrationEventService> _logger;

    public OrderingIntegrationEventService(
        OrderingContext orderingContext,
        IKafkaProducer producer,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
        ILogger<OrderingIntegrationEventService> logger)
    {
        _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _eventLogService = _integrationEventLogServiceFactory(_orderingContext.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void PublishEvent(KafkaIntegrationEvent evt)
    {
        try
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Topic, evt.Message.GetGenericTypeName());

            _producer.Produce(evt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Topic, evt.Message.GetGenericTypeName());
            throw;
        }
    }
    
    // public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    // {
    //     var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);
    //
    //     foreach (var logEvt in pendingLogEvents)
    //     {
    //         _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);
    //
    //         try
    //         {
    //             await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
    //             _eventBus.Publish(logEvt.IntegrationEvent);
    //             await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);
    //
    //             await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
    //         }
    //     }
    // }
    //
    // public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    // {
    //     _logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);
    //
    //     await _eventLogService.SaveEventAsync(evt, _orderingContext.GetCurrentTransaction());
    // }
}
