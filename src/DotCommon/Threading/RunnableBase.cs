namespace DotCommon.Threading
{
    public abstract class RunnableBase : IRunnable
    {
        public bool IsRunning { get { return _isRunning; } }

        private volatile bool _isRunning;

        public virtual void Start()
        {
            _isRunning = true;
        }

        public virtual void Stop()
        {
            _isRunning = false;
        }

        public virtual void WaitToStop()
        {

        }
    }
}
