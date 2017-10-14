using DotCommon.Threading.BackgroundWorkers;
using DotCommon.Threading.Timers;
using System;

namespace DotCommon.ConsoleTest
{
    public class TestBackgroundWorker : PeriodicBackgroundWorkerBase
    {
        public TestBackgroundWorker(DotCommonTimer timer) : base(timer)
        {
            timer.Period = 1000;
            //  timer.RunOnStart = true;
        }

        protected override void DoWork()
        {
            Logger.Info("111");
            Console.WriteLine("Work");
        }
    }
}
