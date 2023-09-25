using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

// Integration Events notes:
// An Event is “something that has happened in the past”, therefore its name has to be
// An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
public record OrderStartedIntegrationEvent(string Key) : KafkaIntegrationEvent(KafkaConstants.OrderingTopicName, Key, new OrderStartedProto { UserId = Key });
