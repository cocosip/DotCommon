using System;

namespace DotCommon.Scheduling
{
    /// <summary>调度器
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>开始一个调度任务
        /// </summary>
        /// <param name="name">任务名</param>
        /// <param name="action">任务操作</param>
        /// <param name="dueTime">在多久时间后开始</param>
        /// <param name="period">执行时间间隔</param>
        void StartTask(string name, Action action, int dueTime, int period);

        /// <summary>根据任务名停止调度任务
        /// </summary>
        /// <param name="name">任务名</param>
        void StopTask(string name);
    }
}
