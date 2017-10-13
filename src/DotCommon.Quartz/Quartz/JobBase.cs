using DotCommon.Dependency;
using DotCommon.Logging;
using Quartz;
using System.Threading.Tasks;

namespace DotCommon.Quartz
{
    public abstract class JobBase : IJob
    {
        public ILogger Logger { protected get; set; }
        protected JobBase()
        {
            Logger = IocManager.GetContainer().Resolve<ILoggerFactory>().Create(typeof(JobBase));
        }

        public abstract Task Execute(IJobExecutionContext context);

    }
}
