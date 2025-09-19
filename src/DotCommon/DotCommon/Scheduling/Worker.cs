using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// Represents a background worker that repeatedly executes a specific method.
    /// </summary>
    public class Worker
    {
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly Action _action;
        private Status _status;

        /// <summary>
        /// Gets the name of the current worker's operation.
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// Initializes a new worker with the specified operation.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown when logger, actionName, or action is null.</exception>
        public Worker(ILogger<Worker> logger, string actionName, Action action)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ActionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _status = Status.Initial;
        }

        /// <summary>
        /// Starts the worker if it is not already running.
        /// </summary>
        /// <returns>The current worker instance.</returns>
        public Worker Start()
        {
            lock (_lockObject)
            {
                if (_status == Status.Running)
                {
                    _logger.LogInformation("Worker '{WorkerName}' is already running.", ActionName);
                    return this;
                }

                _status = Status.Running;

                var thread = new Thread(Loop)
                {
                    Name = $"{ActionName}.Worker",
                    IsBackground = true
                };

                thread.Start();

                _logger.LogInformation("Worker '{WorkerName}' started.", ActionName);
                return this;
            }
        }

        /// <summary>
        /// Requests the worker to stop.
        /// </summary>
        /// <returns>The current worker instance.</returns>
        public Worker Stop()
        {
            lock (_lockObject)
            {
                if (_status == Status.StopRequested)
                {
                    _logger.LogInformation("Stop already requested for worker '{WorkerName}'.", ActionName);
                    return this;
                }

                _status = Status.StopRequested;
                _logger.LogInformation("Stop requested for worker '{WorkerName}'.", ActionName);
                return this;
            }
        }

        /// <summary>
        /// The main loop of the worker.
        /// </summary>
        private void Loop()
        {
            _logger.LogDebug("Worker '{WorkerName}' loop started.", ActionName);

            while (_status == Status.Running)
            {
                try
                {
                    _action();
                }
                catch (ThreadAbortException)
                {
                    _logger.LogInformation(
                        "Worker thread caught ThreadAbortException, trying to reset. Worker name: {WorkerName}",
                        ActionName);

                    Thread.ResetAbort();

                    _logger.LogInformation(
                        "Worker thread ThreadAbortException resetted. Worker name: {WorkerName}",
                        ActionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Worker thread has exception. Worker name: {WorkerName}",
                        ActionName);
                }
            }

            _logger.LogDebug("Worker '{WorkerName}' loop ended.", ActionName);
        }

        /// <summary>
        /// Enum representing the worker status.
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// Initial state.
            /// </summary>
            Initial,

            /// <summary>
            /// Running state.
            /// </summary>
            Running,

            /// <summary>
            /// Stop requested state.
            /// </summary>
            StopRequested
        }
    }
}