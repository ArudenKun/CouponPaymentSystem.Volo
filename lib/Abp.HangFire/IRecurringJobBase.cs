using Abp.Threading.BackgroundWorkers;

namespace Abp.HangFire;

public interface IRecurringJobBase : IBackgroundWorker
{
    abstract string Id { get; }
    abstract string CronSchedule { get; }
}
