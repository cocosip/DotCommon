using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Threading
{
    public interface IRunnable
    {
        /// <summary>
        /// Starts the service.
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the service.
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
