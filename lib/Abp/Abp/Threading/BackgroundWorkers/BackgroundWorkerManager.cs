using Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Threading.BackgroundWorkers;

/// <summary>
/// Implements <see cref="IBackgroundWorkerManager"/>.
/// </summary>
public class BackgroundWorkerManager
    : RunnableBase,
        IBackgroundWorkerManager,
        ISingletonDependency,
        IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IDisposableDependencyServiceWrapper<IBackgroundWorker>> _backgroundJobs;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
    /// </summary>
    public BackgroundWorkerManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _backgroundJobs = new List<IDisposableDependencyServiceWrapper<IBackgroundWorker>>();
    }

    public override void Start()
    {
        base.Start();

        _backgroundJobs.ForEach(job => job.Service.Start());
    }

    public override void Stop()
    {
        _backgroundJobs.ForEach(job => job.Service.Stop());

        base.Stop();
    }

    public override void WaitToStop()
    {
        _backgroundJobs.ForEach(job => job.Service.WaitToStop());

        base.WaitToStop();
    }

    public void Add<TBackgroundJob>()
        where TBackgroundJob : IBackgroundWorker
    {
        _backgroundJobs.Add(_serviceProvider.GetRequiredServiceAsDisposable<TBackgroundJob>());

        if (IsRunning)
        {
            worker.Start();
        }
    }

    private bool _isDisposed;

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        _backgroundJobs.ForEach(_serviceProvider.Release);
        _backgroundJobs.Clear();
    }
}
