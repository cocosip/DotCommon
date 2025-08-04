namespace System.Threading.Tasks
{
    /// <summary>
    /// Provides extension methods for <see cref="TaskFactory"/>.
    /// </summary>
    public static class TaskFactoryExtensions
    {
        /// <summary>
        /// Starts a new task after a specified delay, executing the provided <paramref name="action"/>.
        /// </summary>
        /// <param name="factory">The <see cref="TaskFactory"/> used to create and schedule the task.</param>
        /// <param name="millisecondsDelay">The delay in milliseconds before starting the task. Must be non-negative.</param>
        /// <param name="action">The <see cref="Action"/> to execute after the delay.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the delayed execution of the specified action.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="factory"/> or <paramref name="action"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="millisecondsDelay"/> is less than zero.
        /// </exception>
        public static Task StartDelayedTask(this TaskFactory factory, int millisecondsDelay, Action action)
        {
            // Validate arguments
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            if (millisecondsDelay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Fast-path for already canceled tokens to avoid unnecessary allocations.
            if (factory.CancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(factory.CancellationToken);
            }

            // Use Task.Delay, which is the standard, reliable, and efficient way to handle a cancellable delay.
            // This avoids manual timer management and potential race conditions.
            return Task.Delay(millisecondsDelay, factory.CancellationToken)
                .ContinueWith(_ => action(), factory.CancellationToken,
                              TaskContinuationOptions.OnlyOnRanToCompletion,
                              factory.Scheduler ?? TaskScheduler.Current);
        }
    }
}