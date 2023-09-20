﻿using Confluent.Kafka;
using Google.Protobuf;
using Google.Protobuf.Collections;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.eShopOnContainers.Kafka.Producers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IKafkaProducer _producer;

    public TestController(IKafkaProducer producer)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
    }

    [HttpGet]
    [Route("catalog")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void ProduceCatalogIntegrationEvent()
    {
        var message = new ProductPriceChangedProto { ProductId = 1, NewPrice = 10, OldPrice = 5 };
        var kafkaEvent = new KafkaIntegrationEvent(KafkaTopics.Catalog, "2", message, Array.Empty<KeyValuePair<string, string>>());
        _producer.Produce(kafkaEvent);
    }

    [HttpGet]
    [Route("orders")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void ProduceOrderIntegrationEvent()//OrderEvents.OneofOrderOneofCase eventType)
    {
        // var orderEvent = new OrderEvents();
        IMessage testEvent = null;
        // switch (eventType)
        // {
        //     case OrderEvents.OneofOrderOneofCase.None:
        //         return;
        //     case OrderEvents.OneofOrderOneofCase.OrderStockConfirmedIntegrationEvent:
        //     {
        //         var order = new OrderStockConfirmedIntegrationEventProto { OrderId = 1 };
        //         orderEvent.OrderStockConfirmedIntegrationEvent = order;
        //         testEvent = order;
        //     }
        //         break;
        //     case OrderEvents.OneofOrderOneofCase.OrderStockRejectedIntegrationEvent:
        //     {
        //         var order = new OrderStockRejectedIntegrationEventProto { OrderId = 1, };
        //         var productList = GetRandomNumbers(1, 20, 10).Select(x => new ConfirmedOrderStockItemProto
        //         {
        //             ProductId = x, HasStock = false
        //         });
        //         order.OrderStockItems.AddRange(productList);
        //         orderEvent.OrderStockRejectedIntegrationEvent = order;
        //     }
        //         break;
        //     case OrderEvents.OneofOrderOneofCase.OrderStatusChangedToPaidIntegrationEvent:
        //     {
        //         var order = new OrderStatusChangedToPaidIntegrationEventProto { OrderId = 1 };
        //         var rng = new Random();
        //         var productList = GetRandomNumbers(1, 20, 10).Select(x => new OrderStockItemProto()
        //         {
        //             
        //             ProductId = x,
        //             Units = rng.Next(1, 100)
        //         });
        //         order.OrderStockItems.AddRange(productList);
        //         orderEvent.OrderStatusChangedToPaidIntegrationEvent = order;
        //     }
        //         break;
        //     case OrderEvents.OneofOrderOneofCase.OrderStatusChangedToAwaitingValidationIntegrationEvent:
        //     {
        //         var order = new OrderStatusChangedToAwaitingValidationIntegrationEventProto { OrderId = 1 };
        //         var rng = new Random();
        //         var productList = GetRandomNumbers(1, 20, 10).Select(x => new OrderStockItemProto()
        //         {
        //             
        //             ProductId = x,
        //             Units = rng.Next(1, 100)
        //         });
        //         order.OrderStockItems.AddRange(productList);
        //         orderEvent.OrderStatusChangedToAwaitingValidationIntegrationEvent = order;
        //     }
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        // }
        
        // var integrationEvent = new KafkaIntegrationEvent("Ordering", "TestEvents", orderEvent);
        // _kafkaEventBus.Publish(integrationEvent);
        var protoEvent = new OrderStatusChangedToAwaitingValidationProto()
        {
            OrderId = 1
        };
        var message = new KafkaIntegrationEvent(KafkaTopics.OrderStatus, "1", protoEvent, Array.Empty<KeyValuePair<string, string>>());
        _producer.Produce(message);
    }
    
    private List<int> GetRandomNumbers(int from,int to,int numberOfElement)
    {
        var random = new Random();
        HashSet<int> numbers = new HashSet<int>();
        while (numbers.Count < numberOfElement)
        {
            numbers.Add(random.Next(from, to));
        }
        return numbers.ToList();
    }
}
