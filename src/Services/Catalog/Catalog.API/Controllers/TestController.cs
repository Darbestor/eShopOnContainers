using Confluent.Kafka;
using Google.Protobuf;
using Google.Protobuf.Collections;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IProducerAccessor _producerAccessor;
    // private readonly IKafkaEventBus _kafkaEventBus;

    public TestController(IProducerAccessor producerAccessor)//IKafkaEventBus kafkaEventBus)
    {
        _producerAccessor = producerAccessor ?? throw new ArgumentNullException(nameof(producerAccessor));
        // _kafkaEventBus = kafkaEventBus ?? throw new ArgumentNullException(nameof(kafkaEventBus));
    }

    [HttpGet]
    [Route("catalog")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void ProduceCatalogIntegrationEvent()
    {
        var protoPayload = new ProductPriceChangedProtobuf { ProductId = 1, NewPrice = 10, OldPrice = 5 };
        var producer = _producerAccessor.GetProducer(nameof(ProductPriceChangedProtobuf));
        producer.Produce("1", protoPayload);
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
        var protoEvent = new OrderStatusChangedToPaidIntegrationEventProto
        {
            OrderId = 1,
        };
        var producer = _producerAccessor.GetProducer("Ordering-producer");
        producer.Produce("1", protoEvent);
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
