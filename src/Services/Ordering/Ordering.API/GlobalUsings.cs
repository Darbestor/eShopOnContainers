﻿global using System.Data.Common;
global using System.Data.SqlClient;
global using System.Runtime.Serialization;
global using Azure.Identity;
global using Dapper;
global using FluentValidation;
global using Google.Protobuf.Collections;
global using Grpc.Core;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.eShopOnContainers.Kafka.Consumers;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
global using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
global using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
global using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
global using Microsoft.eShopOnContainers.Services.Ordering.API;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Behaviors;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Models;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Validations;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Extensions;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure;
global using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Services;
global using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
global using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
global using Microsoft.eShopOnContainers.Services.Ordering.Domain.Events;
global using Microsoft.eShopOnContainers.Services.Ordering.Domain.Exceptions;
global using Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork;
global using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure;
global using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Idempotency;
global using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
global using Microsoft.Extensions.Options;
global using KafkaFlow;
global using Polly;
global using Polly.Retry;
global using Services.Common;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using AppCommand = Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
global using ApiModels = Microsoft.eShopOnContainers.Services.Ordering.API.Application.Models;
