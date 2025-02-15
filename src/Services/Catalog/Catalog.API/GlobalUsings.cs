﻿
global using System.Globalization;
global using System.IO.Compression;
global using System.Text.RegularExpressions;
global using Catalog.API.Apis;
global using Grpc.Core;
global using KafkaFlow;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
global using Microsoft.eShopOnContainers.Services.Catalog.API;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Extensions;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Grpc;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.EntityConfigurations;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.Exceptions;
global using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents;
global using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
global using Microsoft.eShopOnContainers.Services.Catalog.API.Model;
global using Microsoft.eShopOnContainers.Services.Catalog.API.ViewModel;
global using Microsoft.Extensions.Options;
global using Polly;
global using Polly.Retry;
global using Services.Common;
global using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog;
global using Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering;

