using DotCommon.Threading.BackgroundWorkers;
using DotCommon.Threading.Timers;

namespace DotCommon.Test.Threading.BackgroundWorkers
{
    public class TestBackgroundWorker : PeriodicBackgroundWorkerBase
    {
        protected TestBackgroundWorker(DotCommonTimer timer) : base(timer)
        {

        }

        protected override void DoWork()
        {
            Logger.Info($"DoWork....");
        }
    }
}
