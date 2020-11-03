using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// Represent a background worker that will repeatedly execute a specific method.
    /// </summary>
    public class Worker
    {
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly Action _action;
        private Status _status;

        /// <summary>Returns the action name of the current worker.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Initialize a new worker with the specified action.
        /// </summary>
        public Worker(ILogger<Worker> logger, string actionName, Action action)
        {
            _logger = logger;
            ActionName = actionName;
            _action = action;
            _status = Status.Initial;
        }

        /// <summary>
        /// Start the worker if it is not running.
        /// </summary>
        public Worker Start()
        {
            lock (_lockObject)
            {
                if (_status == Status.Running)
                {
                    return this;
                }

                _status = Status.Running;
                new Thread(Loop)
                {
                    Name = $"{ActionName}.Worker",
                    IsBackground = true
                }.Start(this);

                return this;
            }
        }
        /// <summary>Request to stop the worker.
        /// </summary>
        public Worker Stop()
        {
            lock (_lockObject)
            {
                if (_status == Status.StopRequested)
                {
                    return this;
                }
                _status = Status.StopRequested;

                return this;
            }
        }

        private void Loop(object data)
        {
            var worker = (Worker)data;

            while (worker._status == Status.Running)
            {
                try
                {
                    _action();
                }
                catch (ThreadAbortException)
                {
                    _logger.LogInformation("Worker thread caught ThreadAbortException, try to resetting, actionName:{0}", ActionName);
                    Thread.ResetAbort();
                    _logger.LogInformation("Worker thread ThreadAbortException resetted, actionName:{0}", ActionName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Worker thread has exception, actionName:{ActionName},error:{ex.Message}");
                }
            }
        }

        enum Status
        {
            Initial,
            Running,
            StopRequested
        }
    }
}
