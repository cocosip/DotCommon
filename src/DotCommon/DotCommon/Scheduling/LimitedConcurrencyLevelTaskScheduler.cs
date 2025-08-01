using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// 限制并发级别的任务调度器
    /// 该调度器确保同时运行的任务数量不超过指定的最大并发级别
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        /// <summary>
        /// 标识当前线程是否正在处理工作项
        /// </summary>
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        /// <summary>
        /// 待执行的任务队列
        /// </summary>
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>();

        /// <summary>
        /// 此调度器允许的最大并发级别
        /// </summary>
        private readonly int _maxDegreeOfParallelism;

        /// <summary>
        /// 当前已排队或正在运行的委托数
        /// </summary>
        private int _delegatesQueuedOrRunning;

        /// <summary>
        /// 初始化具有指定并发级别的LimitedConcurrencyLevelTaskScheduler实例
        /// </summary>
        /// <param name="maxDegreeOfParallelism">此调度器提供的最大并发级别</param>
        /// <exception cref="ArgumentOutOfRangeException">当maxDegreeOfParallelism小于1时抛出</exception>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxDegreeOfParallelism),
                    "Maximum degree of parallelism must be greater than 0.");
            }

            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// 将任务添加到调度器队列中
        /// </summary>
        /// <param name="task">要排队的任务</param>
        protected sealed override void QueueTask(Task task)
        {
            // 将任务添加到待处理队列中
            // 如果当前运行或排队的委托数未达到最大并发级别，则调度新的工作线程
            lock (_tasks)
            {
                _tasks.AddLast(task);

                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        /// <summary>
        /// 通知线程池有待处理的工作
        /// </summary>
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // 标记当前线程正在处理工作项，这使得任务可以内联到此线程执行
                _currentThreadIsProcessingItems = true;

                try
                {
                    // 处理队列中的所有可用项
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // 当没有更多项需要处理时，减少运行计数并退出
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // 从队列中获取下一项
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // 执行从队列中取出的任务
                        TryExecuteTask(item);
                    }
                }
                finally
                {
                    // 完成当前线程的工作项处理
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        /// <summary>
        /// 尝试在当前线程上执行指定任务
        /// </summary>
        /// <param name="task">要执行的任务</param>
        /// <param name="taskWasPreviouslyQueued">任务是否之前已排队</param>
        /// <returns>任务是否可以在当前线程上执行</returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // 如果当前线程未在处理工作项，则不支持内联执行
            if (!_currentThreadIsProcessingItems)
            {
                return false;
            }

            // 如果任务之前已排队，则从队列中移除
            if (taskWasPreviouslyQueued)
            {
                TryDequeue(task);
            }

            // 尝试执行任务
            return TryExecuteTask(task);
        }

        /// <summary>
        /// 尝试从调度器中移除先前计划的任务
        /// </summary>
        /// <param name="task">要移除的任务</param>
        /// <returns>任务是否被找到并移除</returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        /// <summary>
        /// 获取此调度器支持的最大并发级别
        /// </summary>
        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        /// <summary>
        /// 获取当前在此调度器上计划的任务枚举
        /// </summary>
        /// <returns>当前计划的任务枚举</returns>
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (!lockTaken)
                {
                    throw new NotSupportedException("Scheduled tasks list is currently locked.");
                }

                return _tasks.ToArray();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_tasks);
                }
            }
        }
    }
}