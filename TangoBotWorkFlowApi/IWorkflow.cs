using System.Threading;
using System.Threading.Tasks;

namespace TangoBotApi.Workflows
{
    /// <summary>
    /// Defines a workflow consisting of multiple tasks.
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Executes the workflow.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
