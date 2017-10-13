using Quartz;

namespace DotCommon.Quartz.Configuration
{
    public interface IDotCommonQuartzConfiguration
    {
        IScheduler Scheduler { get; }
    }
}
