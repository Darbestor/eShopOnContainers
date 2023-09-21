namespace Ordering.BackgroundTasks.Hangfire;

public interface IHangfireServer : IDisposable
{
    Task RunServerAsync(CancellationToken cancellationToken = default);
}
