using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// Schedule service implementation for managing timed tasks.
    /// </summary>
    public class ScheduleService : IScheduleService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly object _syncObject = new object();
        private readonly Dictionary<string, TimerBasedTask> _taskDict = new Dictionary<string, TimerBasedTask>();
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
        public ScheduleService(ILogger<ScheduleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Starts a scheduled task.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="dueTime">The delay before the first execution in milliseconds.</param>
        /// <param name="period">The interval between executions in milliseconds.</param>
        /// <exception cref="ArgumentNullException">Thrown when name or action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when name is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when dueTime or period is negative.</exception>
        public void StartTask(string name, Action action, int dueTime, int period)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Task name cannot be null or empty.", nameof(name));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (dueTime < 0)
                throw new ArgumentOutOfRangeException(nameof(dueTime), "Due time cannot be negative.");

            if (period < 0)
                throw new ArgumentOutOfRangeException(nameof(period), "Period cannot be negative.");

            lock (_syncObject)
            {
                // If task already exists, log a warning and return
                if (_taskDict.ContainsKey(name))
                {
                    _logger.LogWarning("Task with name '{TaskName}' already exists and will not be started again.", name);
                    return;
                }

                var timer = new Timer(TaskCallback, name, Timeout.Infinite, Timeout.Infinite);
                var task = new TimerBasedTask
                {
                    Name = name,
                    Action = action,
                    Timer = timer,
                    DueTime = dueTime,
                    Period = period,
                    Stopped = false
                };

                _taskDict.Add(name, task);
                timer.Change(dueTime, period);

                _logger.LogInformation(
                    "Task '{TaskName}' started with due time {DueTime}ms and period {Period}ms.",
                    name, dueTime, period);
            }
        }

        /// <summary>
        /// Stops and removes a scheduled task by name.
        /// </summary>
        /// <param name="name">The name of the task to stop.</param>
        /// <exception cref="ArgumentNullException">Thrown when name is null.</exception>
        public void StopTask(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            lock (_syncObject)
            {
                if (_taskDict.TryGetValue(name, out TimerBasedTask task))
                {
                    task.Stopped = true;
                    task.Timer?.Dispose();
                    _taskDict.Remove(name);

                    _logger.LogInformation("Task '{TaskName}' stopped and removed.", name);
                }
                else
                {
                    _logger.LogWarning("Task '{TaskName}' not found and cannot be stopped.", name);
                }
            }
        }

        /// <summary>
        /// Timer callback method.
        /// </summary>
        /// <param name="state">The task name.</param>
        private void TaskCallback(object state)
        {
            var taskName = (string)state;

            if (!_taskDict.TryGetValue(taskName, out TimerBasedTask task))
            {
                _logger.LogWarning("Task '{TaskName}' not found in dictionary during callback execution.", taskName);
                return;
            }

            try
            {
                if (!task.Stopped)
                {
                    // Pause the timer to prevent reentrancy during task execution
                    task.Timer?.Change(Timeout.Infinite, Timeout.Infinite);

                    // Execute the task
                    task.Action?.Invoke();
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignore ObjectDisposedException
                _logger.LogDebug("Timer for task '{TaskName}' was disposed during execution.", task.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while executing task '{TaskName}'. Due time: {DueTime}ms, Period: {Period}ms.",
                    task.Name, task.DueTime, task.Period);
            }
            finally
            {
                try
                {
                    // Restart the timer if the task is not stopped
                    if (!task.Stopped)
                    {
                        task.Timer?.Change(task.Period, task.Period);
                    }
                }
                catch (ObjectDisposedException)
                {
                    _logger.LogDebug("Timer for task '{TaskName}' was disposed while resetting.", task.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Exception occurred while resetting timer for task '{TaskName}'. Due time: {DueTime}ms, Period: {Period}ms.",
                        task.Name, task.DueTime, task.Period);
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the ScheduleService.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the ScheduleService and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Stop all timers first to prevent new callbacks
                    lock (_syncObject)
                    {
                        foreach (var task in _taskDict.Values)
                        {
                            task.Timer?.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                    }

                    // Now dispose of timers and clear the dictionary
                    lock (_syncObject)
                    {
                        foreach (var task in _taskDict.Values)
                        {
                            task.Stopped = true;
                            task.Timer?.Dispose();
                        }
                        _taskDict.Clear();
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Timer-based task information.
        /// </summary>
        private class TimerBasedTask
        {
            /// <summary>
            /// Gets or sets the task name.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the task action.
            /// </summary>
            public Action? Action { get; set; }

            /// <summary>
            /// Gets or sets the timer.
            /// </summary>
            public Timer? Timer { get; set; }

            /// <summary>
            /// Gets or sets the delay before the first execution in milliseconds.
            /// </summary>
            public int DueTime { get; set; }

            /// <summary>
            /// Gets or sets the interval between executions in milliseconds.
            /// </summary>
            public int Period { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the task is stopped.
            /// </summary>
            public bool Stopped { get; set; }
        }
    }
}