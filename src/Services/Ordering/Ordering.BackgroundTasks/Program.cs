
using KafkaFlow;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddApplicationOptions(builder.Configuration);
builder.Services.AddHostedService<GracePeriodManagerService>();
builder.Services.AddKafkaFlow(builder.Configuration, (cluster, config) =>
{
    cluster.CreateTopicIfNotExists(KafkaConstants.OrderGracePeriodTopicName, 3, 1);
});

var app = builder.Build();
var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

app.UseServiceDefaults();

await app.RunAsync();
