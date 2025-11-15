using Abp.Dependency;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Hangfire;

namespace Abp.HangFire;

public class HangfireBackgroundWorkerManager : RunnableBase, IBackgroundWorkerManager, IDisposable
{
    private readonly IIocResolver _iocResolver;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly List<IBackgroundWorker> _backgroundWorkers;

    private bool _isDisposed;

    public HangfireBackgroundWorkerManager(
        IIocResolver iocResolver,
        IRecurringJobManager recurringJobManager
    )
    {
        _iocResolver = iocResolver;
        _recurringJobManager = recurringJobManager;
        _backgroundWorkers = [];
    }

    public void Add(IBackgroundWorker worker)
    {
        switch (worker)
        {
            case IRecurringJob recurringJob:
                _recurringJobManager.AddOrUpdate(
                    recurringJob.Id,
                    () => recurringJob.Execute(),
                    () => recurringJob.CronSchedule
                );
                break;
            case IAsyncRecurringJob asyncRecurringJob:
                _recurringJobManager.AddOrUpdate(
                    asyncRecurringJob.Id,
                    () => asyncRecurringJob.ExecuteAsync(),
                    () => asyncRecurringJob.CronSchedule
                );
                break;
            default:
                _backgroundWorkers.Add(worker);
                break;
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _backgroundWorkers.ForEach(_iocResolver.Release);
        _backgroundWorkers.Clear();
    }
}
