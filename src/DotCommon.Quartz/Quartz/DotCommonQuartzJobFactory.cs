using DotCommon.Dependency;
using DotCommon.Extensions;
using Quartz;
using Quartz.Spi;

namespace DotCommon.Quartz
{
    public class DotCommonQuartzJobFactory : IJobFactory
    {

        public DotCommonQuartzJobFactory()
        {
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return IocManager.GetContainer().Resolve(bundle.JobDetail.JobType).As<IJob>();
        }

        public void ReturnJob(IJob job)
        {
            //释放
            //_iocResolver.Release(job);
        }
    }
}
