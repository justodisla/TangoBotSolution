using System.Threading;
using System.Threading.Tasks;

namespace TangoBotApi.Workflows
{
    /// <summary>
    /// Defines a task within a workflow.
    /// </summary>
    public interface IWorkflowTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
