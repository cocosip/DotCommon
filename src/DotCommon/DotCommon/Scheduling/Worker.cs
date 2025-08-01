using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// 表示一个后台工作者，将重复执行特定方法
    /// </summary>
    public class Worker
    {
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly Action _action;
        private Status _status;

        /// <summary>
        /// 获取当前工作者的操作名称
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// 使用指定操作初始化一个新的工作者
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="action">要执行的操作</param>
        /// <exception cref="ArgumentNullException">当logger、actionName或action为null时抛出</exception>
        public Worker(ILogger<Worker> logger, string actionName, Action action)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ActionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _status = Status.Initial;
        }

        /// <summary>
        /// 如果工作者未运行则启动它
        /// </summary>
        /// <returns>当前工作者实例</returns>
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
        /// 请求停止工作者
        /// </summary>
        /// <returns>当前工作者实例</returns>
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
        /// 工作者的主循环
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
        /// 工作者状态枚举
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// 初始状态
            /// </summary>
            Initial,

            /// <summary>
            /// 运行中
            /// </summary>
            Running,

            /// <summary>
            /// 已请求停止
            /// </summary>
            StopRequested
        }
    }
}