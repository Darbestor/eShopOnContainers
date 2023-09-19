global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
global using Microsoft.eShopOnContainers.Services.Ordering.SignalrHub;
global using Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents.Events;
global using Services.Common;
global using KafkaFlow;
global using Microsoft.eShopOnContainers.Kafka.Consumers;
global using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.OrderStatus;

