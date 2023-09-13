﻿using Google.Protobuf;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;

public class CompoundOrderTypesEventHandler :
    IIntegrationProtobufEventHandler<OrderEvents>
{
    private readonly CatalogContext _catalogContext;
    private readonly ILogger<CompoundOrderTypesEventHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CompoundOrderTypesEventHandler(
        CatalogContext catalogContext,
        ILogger<CompoundOrderTypesEventHandler> logger,
        IServiceProvider serviceProvider)
    {
        _catalogContext = catalogContext;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new ("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            //we're not blocking stock/inventory
            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);

                catalogItem.RemoveStock(orderStockItem.Units);
            }

            await _catalogContext.SaveChangesAsync();

        }
    }

    public async Task Handle(string key, OrderEvents message)
    {
        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", key, message);

        await using var scope = _serviceProvider.CreateAsyncScope();
        IIntegrationProtobufEventHandler exactEvent = message.OneofOrderCase switch
        {
            OrderEvents.OneofOrderOneofCase.None => null,
            OrderEvents.OneofOrderOneofCase.OrderStockConfirmedIntegrationEvent => scope.ServiceProvider.GetService<IIntegrationProtobufEventHandler<OrderStockConfirmedIntegrationEventProto>>(),
            OrderEvents.OneofOrderOneofCase.OrderStockRejectedIntegrationEvent => scope.ServiceProvider.GetService<IIntegrationProtobufEventHandler<OrderStockRejectedIntegrationEventProto>>(),
            OrderEvents.OneofOrderOneofCase.OrderStatusChangedToPaidIntegrationEvent => scope.ServiceProvider.GetService<IIntegrationProtobufEventHandler<OrderStatusChangedToPaidIntegrationEventProto>>(),
            OrderEvents.OneofOrderOneofCase.OrderStatusChangedToAwaitingValidationIntegrationEvent => scope.ServiceProvider.GetService<IIntegrationProtobufEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEventProto>>(),
            _ => throw new ArgumentOutOfRangeException()
        };
        if (exactEvent == null)
        {
            _logger.LogInformation("Handler for integration event: {IntegrationEventId} not found", key);
            return;
        }

        await exactEvent.Handle(key, message);
    }

    public async Task Handle(string key, IMessage message)
    {
        await Handle(key, message as OrderEvents);
    }
}
