namespace DotCommon.Threading
{
    public interface IRunnable
    {
        void Start();

        void Stop();

        void WaitToStop();
    }
}
