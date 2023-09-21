using Hangfire;

namespace Ordering.BackgroundTasks.Hangfire;

public class HangfireJobActivator : JobActivator
{
    private readonly IServiceProvider _provider;

    public HangfireJobActivator(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public override object ActivateJob(Type jobType)
    {
        return _provider.GetRequiredService(jobType);
    }
}
