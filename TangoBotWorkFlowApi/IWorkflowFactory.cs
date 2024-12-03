namespace TangoBotApi.Workflows
{
    /// <summary>
    /// Factory for creating workflows.
    /// </summary>
    public interface IWorkflowFactory
    {
        /// <summary>
        /// Creates a new workflow.
        /// </summary>
        /// <returns>A new workflow instance.</returns>
        IWorkflow CreateWorkflow();
    }
}
