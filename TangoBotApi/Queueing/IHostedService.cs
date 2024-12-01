using System.Threading;
using System.Threading.Tasks;

namespace TangoBotApi.Infrastructure
{
    /// <summary>
    /// Defines methods for starting and stopping hosted services.
    /// </summary>
    public interface IHostedService
    {
        /// <summary>
        /// Starts the hosted service.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the hosted service.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}


