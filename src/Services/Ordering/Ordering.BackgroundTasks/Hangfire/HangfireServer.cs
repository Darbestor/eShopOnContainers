using Hangfire;
using Microsoft.Extensions.Options;

namespace Ordering.BackgroundTasks.Hangfire;

public class HangfireServer : IHangfireServer
{
    private readonly object _lockObject = new();
    private readonly ILogger<HangfireServer> _logger;
    private readonly BackgroundTaskSettings _options;
    private BackgroundJobServer _backgroundJobServer;
    private bool _serverStarted;

    public HangfireServer(ILogger<HangfireServer> logger, IOptions<BackgroundTaskSettings> options)
    {
        _logger = logger;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task RunServerAsync(CancellationToken cancellationToken)
    {
        if (_serverStarted)
        {
            _logger.LogError("Hangfire server already running");
            return;
        }

        lock (_lockObject)
        {
            if (_serverStarted)
            {
                _logger.LogError("Hangfire server already running");
                return;
            }

            Initialize();
        }

        try
        {
            _logger.LogInformation("Hangfire server started");
            await _backgroundJobServer.WaitForShutdownAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while Hangfire server was running");
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _backgroundJobServer?.Dispose();
    }

    private void Initialize()
    {
        var backgroundJobServerOptions = new BackgroundJobServerOptions
        {
            SchedulePollingInterval = TimeSpan.FromMilliseconds(_options.GracePeriodTime)
        };
        _backgroundJobServer = new BackgroundJobServer(backgroundJobServerOptions);
        _serverStarted = true;
    }
}
