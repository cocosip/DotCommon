using System;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// Schedule service implementation for managing timed tasks.
    /// </summary>
    public interface IScheduleService
    {

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
        void StartTask(string name, Action action, int dueTime, int period);

        /// <summary>
        /// Stops and removes a scheduled task by name.
        /// </summary>
        /// <param name="name">The name of the task to stop.</param>
        /// <exception cref="ArgumentNullException">Thrown when name is null.</exception>
        void StopTask(string name);
    }
}