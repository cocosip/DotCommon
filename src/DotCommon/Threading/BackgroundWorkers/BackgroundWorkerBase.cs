using DotCommon.Dependency;
using DotCommon.Logging;

namespace DotCommon.Threading.BackgroundWorkers
{
    public abstract class BackgroundWorkerBase : RunnableBase, IBackgroundWorker
    {
        public ILogger Logger { protected get; set; }
        protected BackgroundWorkerBase()
        {
            Logger = IocManager.GetContainer().Resolve<ILoggerFactory>().Create(typeof(BackgroundWorkerBase));
        }

        public override void Start()
        {
            base.Start();
            Logger.Debug("Start background worker: " + ToString());
        }

        public override void Stop()
        {
            base.Stop();
            Logger.Debug("Stop background worker: " + ToString());
        }

        public override void WaitToStop()
        {
            base.WaitToStop();
            Logger.Debug("WaitToStop background worker: " + ToString());
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}
