using System;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// 调度服务接口，用于管理定时任务
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// 启动一个调度任务
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="action">任务执行的操作</param>
        /// <param name="dueTime">首次执行前的延迟时间（毫秒）</param>
        /// <param name="period">执行间隔时间（毫秒）</param>
        void StartTask(string name, Action action, int dueTime, int period);

        /// <summary>
        /// 停止并移除指定名称的调度任务
        /// </summary>
        /// <param name="name">要停止的任务名称</param>
        void StopTask(string name);
    }
}