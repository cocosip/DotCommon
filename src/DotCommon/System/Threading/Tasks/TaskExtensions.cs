namespace System.Threading.Tasks
{
    /// <summary>
    /// Provides extension methods for the <see cref="Task"/> and <see cref="Task{TResult}"/> classes.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Waits for the task to complete and returns the result. This is a blocking operation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">The task to wait on.</param>
        /// <param name="timeoutMillis">The number of milliseconds to wait, or <see cref="Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
        /// <returns>The result of the task, or the default value of <typeparamref name="TResult" /> if the task does not complete within the specified time.</returns>
        /// <exception cref="AggregateException">Thrown if the task completes with an exception.</exception>
        public static TResult? WaitResult<TResult>(this Task<TResult> task, int timeoutMillis)
        {
            if (task.Wait(timeoutMillis))
            {
                return task.Result; // This will re-throw any exception that occurred in the task
            }
            return default;
        }

        /// <summary>
        /// Throws a <see cref="TimeoutException" /> if the task does not complete within the specified time.
        /// </summary>
        /// <param name="task">The task to wait on.</param>
        /// <param name="millisecondsDelay">The number of milliseconds to wait.</param>
        /// <exception cref="TimeoutException">Thrown if the task does not complete within the specified time.</exception>
        /// <exception cref="Exception">Thrown if the task completes with an exception.</exception>
        public static async Task TimeoutAfter(this Task task, int millisecondsDelay)
        {
            using (var timeoutCancellationTokenSource = new Threading.CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    // Task completed, propagate exceptions
                    timeoutCancellationTokenSource.Cancel();
                    await task;
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="TimeoutException" /> if the task does not complete within the specified time.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">The task to wait on.</param>
        /// <param name="millisecondsDelay">The number of milliseconds to wait.</param>
        /// <returns>The result of the task.</returns>
        /// <exception cref="TimeoutException">Thrown if the task does not complete within the specified time.</exception>
        /// <exception cref="Exception">Thrown if the task completes with an exception.</exception>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int millisecondsDelay)
        {
            using (var timeoutCancellationTokenSource = new Threading.CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    // Task completed, propagate exceptions without awaiting again
                    timeoutCancellationTokenSource.Cancel();
                    // Use await to properly propagate exceptions
                    return await task;
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}

