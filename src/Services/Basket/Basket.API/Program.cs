var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);

builder.Services.AddTransient<IBasketRepository, RedisBasketRepository>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

var app = builder.Build();
var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

app.UseServiceDefaults();

app.MapGrpcService<BasketService>();
app.MapControllers();


await app.RunAsync();
