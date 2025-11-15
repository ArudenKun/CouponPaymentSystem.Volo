using Abp.Threading.BackgroundWorkers;
using AutoInterfaceAttributes;

namespace Abp.HangFire;

[AutoInterface(Name = "IRecurringJob", Inheritance = [typeof(IRecurringJobBase)])]
public abstract class RecurringJobBase : BackgroundWorkerBase, IRecurringJob
{
    public abstract string Id { get; }
    public abstract string CronSchedule { get; }

    public abstract void Execute();

    public sealed override void Start() => base.Start();

    public sealed override void Stop() => base.Stop();

    public sealed override void WaitToStop() => base.WaitToStop();
}
