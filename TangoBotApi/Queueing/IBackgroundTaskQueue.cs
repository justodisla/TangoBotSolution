using System;
using System.Threading.Tasks;

namespace TangoBotApi.Infrastructure
{
    /// <summary>
    /// Provides functionalities to manage background tasks.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Enqueues a background task.
        /// </summary>
        /// <param name="task">The background task to enqueue.</param>
        void EnqueueBackgroundTask(Func<CancellationToken, Task> task);

        /// <summary>
        /// Dequeues a background task.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the dequeued background task.</returns>
        Task<Func<CancellationToken, Task>> DequeueBackgroundTaskAsync(CancellationToken cancellationToken);
    }
}


