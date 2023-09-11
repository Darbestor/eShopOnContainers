﻿global using System.Net.Sockets;
global using System.Text;
global using System.Text.Json;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
global using Microsoft.Extensions.Logging;
global using Polly;
global using Polly.Retry;
global using Confluent.Kafka;
