using DotCommon.Threading.Timers;
using System;

namespace DotCommon.Threading.BackgroundWorkers
{
    public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected readonly DotCommonTimer Timer;

        protected PeriodicBackgroundWorkerBase(DotCommonTimer timer)
        {
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        public override void Start()
        {
            base.Start();
            Timer.Start();
        }

        public override void Stop()
        {
            Timer.Stop();
            base.Stop();
        }

        public override void WaitToStop()
        {
            Timer.WaitToStop();
            base.WaitToStop();
        }

        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        protected abstract void DoWork();
    }
}
