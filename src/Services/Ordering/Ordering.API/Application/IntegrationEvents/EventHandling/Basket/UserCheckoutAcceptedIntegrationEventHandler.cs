using Microsoft.eShopOnContainers.Kafka.Consumers;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Basket;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling.Basket;

public class UserCheckoutAcceptedIntegrationEventHandler : KafkaConsumerEventHandler<UserCheckoutAcceptedProto>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

    public UserCheckoutAcceptedIntegrationEventHandler(
        IMediator mediator,
        ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
    :base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Integration event handler which starts the create order process
    /// </summary>
    /// <param name="context">KafkaFlow message context</param>
    /// <param name="message">
    /// Integration event message which is sent by the
    /// basket.api once it has successfully process the 
    /// order items.</param>
    /// <returns></returns>
    protected override async Task HandleInternal(IMessageContext context, UserCheckoutAcceptedProto message)
    {
        var result = false;

        if (message.RequestId != Guid.Empty)
        {
            using (_logger.BeginScope(
                       new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", message.RequestId) }))
            {
                var createOrderCommand = new CreateOrderCommand(message.Basket.Items.Select(x => (BasketItem)x).ToList(), message.UserId, message.UserName,
                    message.City, message.Street,
                    message.State, message.Country, message.ZipCode,
                    message.CardNumber, message.CardHolderName, message.CardExpiration.ToDateTime(),
                    message.CardSecurityNumber, message.CardTypeId);

                var requestCreateOrder =
                    new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, message.RequestId);

                _logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCreateOrder.GetGenericTypeName(),
                    nameof(requestCreateOrder.Id),
                    requestCreateOrder.Id,
                    requestCreateOrder);

                result = await _mediator.Send(requestCreateOrder);

                if (result)
                {
                    _logger.LogInformation("CreateOrderCommand suceeded - RequestId: {RequestId}", message.RequestId);
                }
                else
                {
                    _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", message.RequestId);
                }
            }
        }
        else
        {
            _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", message);
        }
    }
}
