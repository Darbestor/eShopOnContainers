var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSignalR(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);
var app = builder.Build();
var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

app.UseServiceDefaults();

app.MapHub<NotificationsHub>("/hub/notificationhub");


await app.RunAsync();
