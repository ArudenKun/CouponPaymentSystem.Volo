namespace Abp.Threading.BackgroundWorkers;

/// <summary>
/// Used to manage background workers.
/// </summary>
public interface IBackgroundWorkerManager : IRunnable
{
    /// <summary>
    /// Adds a new worker. Starts the worker immediately if <see cref="IBackgroundWorkerManager"/> has started.
    /// </summary>
    void Add<TBackgroundWorker>()
        where TBackgroundWorker : IBackgroundWorker;
}
