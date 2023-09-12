﻿using Microsoft.eShopOnContainers.BuildingBlocks.EventBusKafka;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddGrpc();
builder.Services.AddControllers();

// Application specific services
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddApplicationOptions(builder.Configuration);
builder.Services.AddIntegrationServices();
// Kafka
builder.Services.AddKafka(builder.Configuration);
builder.Services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
builder.Services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();
builder.Services
    .AddTransient<IIntegrationProtobufEventHandler<ProductPriceChangedIntegrationEventProto>,
        ProductPriceEventHandler>();

var app = builder.Build();

app.UseServiceDefaults();

app.MapPicApi();
app.MapControllers();
app.MapGrpcService<CatalogService>();
var bus = app.Services.GetRequiredService<KafkaManager>();
bus.Subscribe<ProductPriceChangedIntegrationEventProto>("Catalog");
bus.Subscribe<OrderEvents>("Ordering");
var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

// REVIEW: This is done for development ease but shouldn't be here in production
using (var scope = app.Services.CreateScope())
{
    var settings = app.Services.GetService<IOptions<CatalogSettings>>();
    var logger = app.Services.GetService<ILogger<CatalogContextSeed>>();
    scope.ServiceProvider.MigrateDbContext<CatalogContext>((context, sp) =>
    {
        context.Database.Migrate();
        new CatalogContextSeed().SeedAsync(context, app.Environment, settings, logger).Wait();
    });
    
    var integrationEventLogContext = scope.ServiceProvider.GetRequiredService<IntegrationEventLogContext>();
    await integrationEventLogContext.Database.MigrateAsync();
}
await app.RunAsync();
