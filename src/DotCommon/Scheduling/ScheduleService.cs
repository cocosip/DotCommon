using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DotCommon.Scheduling
{
    /// <summary>调度器
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly ILogger _logger;
        private readonly object SyncObject = new object();
        private readonly Dictionary<string, TimerBasedTask> _taskDict = new Dictionary<string, TimerBasedTask>();

        /// <summary>Ctor
        /// </summary>
        public ScheduleService(ILogger<ScheduleService> logger)
        {
            _logger = logger;
        }

        /// <summary>开始一个调度任务
        /// </summary>
        /// <param name="name">任务名</param>
        /// <param name="action">任务操作</param>
        /// <param name="dueTime">在多久时间后开始</param>
        /// <param name="period">执行时间间隔</param>
        public void StartTask(string name, Action action, int dueTime, int period)
        {
            lock (SyncObject)
            {
                if (_taskDict.ContainsKey(name))
                {
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
            }
        }

        /// <summary>根据任务名停止调度任务
        /// </summary>
        /// <param name="name">任务名</param>
        public void StopTask(string name)
        {
            lock (SyncObject)
            {
                TimerBasedTask task;
                if (_taskDict.TryGetValue(name, out task))
                {
                    task.Stopped = true;
                    task.Timer.Dispose();
                    _taskDict.Remove(name);
                }
            }
        }

        private void TaskCallback(object obj)
        {
            var taskName = (string)obj;
            TimerBasedTask task;
            if (_taskDict.TryGetValue(taskName, out task))
            {
                try
                {
                    if (!task.Stopped)
                    {
                        task.Timer.Change(Timeout.Infinite, Timeout.Infinite);
                        task.Action();
                    }
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    throw new Exception($"Task has exception, name: {task.Name}, due: {task.DueTime}, period: {task.Period},error detail:{ex.Message}");
                }
                finally
                {
                    try
                    {
                        if (!task.Stopped)
                        {
                            task.Timer.Change(task.Period, task.Period);
                        }
                    }
                    catch (ObjectDisposedException ex)
                    {
                        _logger.LogError(ex, "Object has disposed,{0}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Timer change has exception, name: {task.Name}, due: {task.DueTime}, period: {task.Period},error detail:{ex.Message}");
                    }
                }
            }
        }

        class TimerBasedTask
        {
            public string Name;
            public Action Action;
            public Timer Timer;
            public int DueTime;
            public int Period;
            public bool Stopped;
        }
    }
}
