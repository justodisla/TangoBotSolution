using System.Threading;
using System.Threading.Tasks;

namespace TangoBotApi.Workflows
{
    /// <summary>
    /// Manages the execution of workflows.
    /// </summary>
    public interface IWorkflowManager
    {
        /// <summary>
        /// Executes a workflow.
        /// </summary>
        /// <param name="workflow">The workflow to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteWorkflowAsync(IWorkflow workflow, CancellationToken cancellationToken);
    }
}
