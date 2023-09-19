
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<PaymentSettings>(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);

builder.Services.AddTransient<OrderStatusChangedToStockConfirmedIntegrationEventHandler>();

var app = builder.Build();
var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

app.UseServiceDefaults();

var eventBus = app.Services.GetRequiredService<IEventBus>();

await app.RunAsync();
