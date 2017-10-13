namespace DotCommon.Threading
{
    public static class RunnableExtensions
    {
        public static void StopAndWaitToStop(this IRunnable runnable)
        {
            runnable.Stop();
            runnable.WaitToStop();
        }
    }
}
