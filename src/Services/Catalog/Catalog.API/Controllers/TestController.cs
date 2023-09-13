using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IKafkaEventBus _kafkaEventBus;

    public TestController(IKafkaEventBus kafkaEventBus)
    {
        _kafkaEventBus = kafkaEventBus ?? throw new ArgumentNullException(nameof(kafkaEventBus));
    }

    [HttpGet]
    [Route("orders")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public void ProduceOrderIntegrationEvent(OrderEvents.OneofOrderOneofCase eventType)
    {
        IMessage message = null;
        switch (eventType)
        {
            case OrderEvents.OneofOrderOneofCase.None:
                return;
            case OrderEvents.OneofOrderOneofCase.OrderStockConfirmedIntegrationEvent:
            {
                var order = new OrderStockConfirmedIntegrationEventProto { OrderId = 1 };
                message = order;       
            }
                break;
            case OrderEvents.OneofOrderOneofCase.OrderStockRejectedIntegrationEvent:
            {
                var order = new OrderStockRejectedIntegrationEventProto { OrderId = 1, };
                var productList = GetRandomNumbers(1, 20, 20).Select(x => new ConfirmedOrderStockItemProto
                {
                    ProductId = x, HasStock = false
                });
                order.OrderStockItems.AddRange(productList);
                message = order;               
            }
                break;
            case OrderEvents.OneofOrderOneofCase.OrderStatusChangedToPaidIntegrationEvent:
            {
                var order = new OrderStatusChangedToPaidIntegrationEventProto { OrderId = 1 };
                var rng = new Random();
                var productList = GetRandomNumbers(1, 20, 20).Select(x => new OrderStockItemProto()
                {
                    
                    ProductId = x,
                    Units = rng.Next(1, 100)
                });
                order.OrderStockItems.AddRange(productList);
            }
                break;
            case OrderEvents.OneofOrderOneofCase.OrderStatusChangedToAwaitingValidationIntegrationEvent:
            {
                var order = new OrderStatusChangedToAwaitingValidationIntegrationEventProto { OrderId = 1 };
                var rng = new Random();
                var productList = GetRandomNumbers(1, 20, 20).Select(x => new OrderStockItemProto()
                {
                    
                    ProductId = x,
                    Units = rng.Next(1, 100)
                });
                order.OrderStockItems.AddRange(productList);
            }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
        }
        var integrationEvent = new KafkaIntegrationEvent() { Message = message, Key = "TestEvents" };
        _kafkaEventBus.Publish("Ordering", integrationEvent);
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
