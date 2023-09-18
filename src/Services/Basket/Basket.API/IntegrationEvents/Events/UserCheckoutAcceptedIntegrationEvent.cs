using Google.Protobuf.WellKnownTypes;
using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Basket;
using CustomerBasket = Microsoft.eShopOnContainers.Services.Basket.API.Model.CustomerBasket;

namespace Basket.API.IntegrationEvents.Events;

public record UserCheckoutAcceptedIntegrationEvent : KafkaIntegrationEvent
{
    public UserCheckoutAcceptedIntegrationEvent(string userId, string userName, string city, string street,
        string state, string country, string zipCode, string cardNumber, string cardHolderName,
        DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, string buyer, Guid requestId,
        CustomerBasket basket) :
        base(
            "Basket",
            userId,
            BuildProto(userId, userName, city, street,
                state, country, zipCode, cardNumber, cardHolderName,
                cardExpiration, cardSecurityNumber, cardTypeId, buyer, requestId, basket),
            Array.Empty<KeyValuePair<string, string>>())
    {
    }

    private static UserCheckoutAcceptedProto BuildProto(string userId, string userName, string city,
        string street,
        string state, string country, string zipCode, string cardNumber, string cardHolderName,
        DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, string buyer, Guid requestId,
        CustomerBasket basket)
    {
        var proto = new UserCheckoutAcceptedProto
        {
            UserId = userId,
            CardExpiration = Timestamp.FromDateTime(cardExpiration),
            CardNumber = cardNumber,
            UserName = userName,
            RequestId = requestId,
            Buyer = buyer,
            CardHolderName = cardHolderName,
            CardSecurityNumber = cardSecurityNumber,
            CardTypeId = cardTypeId,
            City = city,
            ZipCode = zipCode,
            Country = country,
            State = state,
            Street = street,
            Basket = basket
        };

        return proto;
    }

}
