using Webhooks.API.IntegrationEvents.OrderStatus;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddHttpClientServices();
builder.Services.AddIntegrationServices();
builder.Services.AddKafka(builder.Configuration);

builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IGrantUrlTesterService, GrantUrlTesterService>();
builder.Services.AddTransient<IWebhooksRetriever, WebhooksRetriever>();
builder.Services.AddTransient<IWebhooksSender, WebhooksSender>();

var app = builder.Build();
var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

app.UseServiceDefaults();

app.MapControllers();

app.Services.MigrateDbContext<WebhooksContext>((_, __) => { });

await app.RunAsync();
