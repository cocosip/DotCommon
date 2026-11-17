using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Utility
{
    /// <summary>
    /// A double-buffered queue. This queue uses two <see cref="ConcurrentQueue{TMessage}"/> instances
    /// to implement a double-buffering mechanism. One queue (`_inputQueue`) is used for receiving new messages,
    /// and the other (`_processQueue`) is used for processing messages.
    /// Message processing is attempted on each enqueue.
    /// </summary>
    /// <typeparam name="TMessage">The type of messages in the queue.</typeparam>
    public class BufferQueue<TMessage>
    {
        /// <summary>
        /// The most recent exception thrown by <c>handleMessageAction</c>, or null. Kept so a failed
        /// message stays observable even when no <see cref="MessageProcessingFailed"/> subscriber exists.
        /// </summary>
        public Exception? LastError { get; private set; }

        /// <summary>
        /// Raised when <c>handleMessageAction</c> throws.
        /// </summary>
        public event Action<Exception>? MessageProcessingFailed;

        private ConcurrentQueue<TMessage> _inputQueue;
        private ConcurrentQueue<TMessage> _processQueue;
        private readonly Action<TMessage> _handleMessageAction;
        private readonly string _name;
        private int _isProcessingMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferQueue{TMessage}"/> class.
        /// </summary>
        /// <param name="name">The name of the queue, used for logging or error messages.</param>
        /// <param name="handleMessageAction">The delegate used to process a single message.</param>
        public BufferQueue(string name, Action<TMessage> handleMessageAction)
        {
            _name = name;
            _handleMessageAction = handleMessageAction;
            _inputQueue = new ConcurrentQueue<TMessage>();
            _processQueue = new ConcurrentQueue<TMessage>();
        }

        /// <summary>
        /// Enqueues a message into the buffer queue.
        /// The message is added to the input queue, and message processing is attempted.
        /// </summary>
        /// <param name="message">The message to enqueue.</param>
        public void EnqueueMessage(TMessage message)
        {
            _inputQueue.Enqueue(message);
            TryProcessMessages();
            // Removed Thread.Sleep(20) as it's a blocking operation and can impact performance.
            // For backpressure, consider using more advanced concurrency control or bounded queues.
        }

        /// <summary>
        /// Attempts to process messages in the queue.
        /// This method uses <see cref="Interlocked.CompareExchange(ref int, int, int)"/> to ensure that only one thread is processing messages at a time.
        /// If no message processing task is currently running, it starts a new task to process messages from the `_processQueue`.
        /// </summary>
        private void TryProcessMessages()
        {
            // Use Interlocked.CompareExchange to ensure only one thread enters the message processing logic
            if (Interlocked.CompareExchange(ref _isProcessingMessage, 1, 0) == 0)
            {
                // Use Task.Run instead of Task.Factory.StartNew, as Task.Run is the recommended way to run tasks on the thread pool.
                Task.Run(() =>
                {
                    try
                    {
                        // If the processing queue is empty but the input queue has messages, swap the queues.
                        if (_processQueue.Count == 0 && _inputQueue.Count > 0)
                        {
                            SwapInputQueue();
                        }

                        // If there are messages in the processing queue, process them one by one.
                        if (_processQueue.Count > 0)
                        {
                            while (_processQueue.TryDequeue(out TMessage? message))
                            {
                                try
                                {
                                    _handleMessageAction(message);
                                }
                                catch (Exception ex)
                                {
                                    LastError = ex;
                                    MessageProcessingFailed?.Invoke(ex);
                                }
                            }
                        }
                    }
                    finally
                    {
                        // Ensure the flag is reset after processing, allowing other threads to start processing.
                        Interlocked.Exchange(ref _isProcessingMessage, 0);
                        // If new messages were enqueued during processing, attempt to process them again.
                        if (_inputQueue.Count > 0)
                        {
                            TryProcessMessages();
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Swaps the input queue and the processing queue.
        /// This is an atomic operation used to quickly transfer accumulated input messages to the processing queue.
        /// </summary>
        private void SwapInputQueue()
        {
            (_processQueue, _inputQueue) = (_inputQueue, _processQueue);
        }
    }
}
