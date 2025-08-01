using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// 调度服务实现，用于管理定时任务
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly ILogger _logger;
        private readonly object _syncObject = new object();
        private readonly Dictionary<string, TimerBasedTask> _taskDict = new Dictionary<string, TimerBasedTask>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ScheduleService(ILogger<ScheduleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 启动一个调度任务
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="action">任务执行的操作</param>
        /// <param name="dueTime">首次执行前的延迟时间（毫秒）</param>
        /// <param name="period">执行间隔时间（毫秒）</param>
        /// <exception cref="ArgumentNullException">当name或action为null时抛出</exception>
        /// <exception cref="ArgumentException">当name为空字符串时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当dueTime或period为负数时抛出</exception>
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
                // 如果任务已存在，则直接返回
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
        /// 停止并移除指定名称的调度任务
        /// </summary>
        /// <param name="name">要停止的任务名称</param>
        /// <exception cref="ArgumentNullException">当name为null时抛出</exception>
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
        /// 定时器回调方法
        /// </summary>
        /// <param name="state">任务名称</param>
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
                    // 暂停定时器以防止在执行任务时重复触发
                    task.Timer?.Change(Timeout.Infinite, Timeout.Infinite);

                    // 执行任务
                    task.Action?.Invoke();
                }
            }
            catch (ObjectDisposedException)
            {
                // 忽略对象已释放的异常
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
                    // 如果任务未停止，则重新启动定时器
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
        /// 基于定时器的任务信息
        /// </summary>
        private class TimerBasedTask
        {
            /// <summary>
            /// 任务名称
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// 任务操作
            /// </summary>
            public Action? Action { get; set; }

            /// <summary>
            /// 定时器
            /// </summary>
            public Timer? Timer { get; set; }

            /// <summary>
            /// 首次执行前的延迟时间（毫秒）
            /// </summary>
            public int DueTime { get; set; }

            /// <summary>
            /// 执行间隔时间（毫秒）
            /// </summary>
            public int Period { get; set; }

            /// <summary>
            /// 任务是否已停止
            /// </summary>
            public bool Stopped { get; set; }
        }
    }
}