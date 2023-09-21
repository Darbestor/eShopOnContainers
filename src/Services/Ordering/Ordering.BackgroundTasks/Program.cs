using Ordering.BackgroundTasks.Hangfire;

var environmentName = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .AddEnvironmentVariables("NETCORE_")
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton(configuration);
services.AddSingleton<GracePeriodService>();

services.AddLogging(x =>
{
    x.AddConfiguration(configuration)
        .AddConsole();
});
// Shared app insights configuration
services.AddApplicationInsights(configuration);
// Default health checks assume the event bus and self health checks
services.AddDefaultHealthChecks(configuration);
services.AddHealthChecks(configuration);
services.AddApplicationOptions(configuration);
services.AddKafka(configuration);
services.AddHangfireServer();

var provider = services.BuildServiceProvider();
var bus = provider.CreateKafkaBus();
var hangfireServer = provider.GetRequiredService<IHangfireServer>();

await bus.StartAsync();
await hangfireServer.RunServerAsync();
