using Abp.HangFire;
using Abp.Threading.BackgroundWorkers;
using AutoInterfaceAttributes;

namespace Abp;

[AutoInterface(Name = "IAsyncRecurringJob", Inheritance = [typeof(IRecurringJobBase)])]
public abstract class AsyncRecurringJobBase : BackgroundWorkerBase, IAsyncRecurringJob
{
    public abstract string Id { get; }
    public abstract string CronSchedule { get; }
    public abstract Task ExecuteAsync();

    public sealed override void Start() => base.Start();

    public sealed override void Stop() => base.Stop();

    public sealed override void WaitToStop() => base.WaitToStop();
}
