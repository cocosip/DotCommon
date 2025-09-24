using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Scheduling
{
    /// <summary>
    /// A task scheduler that limits the number of concurrent tasks
    /// This scheduler ensures that the number of simultaneously running tasks does not exceed the specified maximum concurrency level
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        /// <summary>
        /// Indicates whether the current thread is processing work items
        /// </summary>
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        /// <summary>
        /// The queue of tasks to be executed
        /// </summary>
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>();

        /// <summary>
        /// The maximum concurrency level allowed by this scheduler
        /// </summary>
        private readonly int _maxDegreeOfParallelism;

        /// <summary>
        /// The number of delegates currently queued or running
        /// </summary>
        private int _delegatesQueuedOrRunning;

        /// <summary>
        /// Initializes a new instance of the LimitedConcurrencyLevelTaskScheduler class with the specified concurrency level
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The maximum concurrency level provided by this scheduler</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when maxDegreeOfParallelism is less than 1</exception>
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
        /// Queues a task to the scheduler
        /// </summary>
        /// <param name="task">The task to be queued</param>
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the pending queue
            // If the number of currently running or queued delegates has not reached the maximum concurrency level, schedule a new worker thread
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
        /// Notifies the thread pool of pending work
        /// </summary>
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Mark that the current thread is processing work items, which allows tasks to be inlined to this thread for execution
                _currentThreadIsProcessingItems = true;

                try
                {
                    // Process all available items in the queue
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to process, decrease the running count and exit
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task retrieved from the queue
                        TryExecuteTask(item);
                    }
                }
                finally
                {
                    // Complete work item processing for the current thread
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        /// <summary>
        /// Attempts to execute the specified task on the current thread
        /// </summary>
        /// <param name="task">The task to execute</param>
        /// <param name="taskWasPreviouslyQueued">Whether the task was previously queued</param>
        /// <returns>Whether the task can be executed on the current thread</returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If the current thread is not processing work items, inline execution is not supported
            if (!_currentThreadIsProcessingItems)
            {
                return false;
            }

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
            {
                TryDequeue(task);
            }

            // Attempt to execute the task
            return TryExecuteTask(task);
        }

        /// <summary>
        /// Attempts to remove a previously scheduled task from the scheduler
        /// </summary>
        /// <param name="task">The task to remove</param>
        /// <returns>Whether the task was found and removed</returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        /// <summary>
        /// Gets the maximum concurrency level supported by this scheduler
        /// </summary>
        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        /// <summary>
        /// Gets an enumerable of the tasks currently scheduled on this scheduler
        /// </summary>
        /// <returns>An enumerable of the currently scheduled tasks</returns>
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