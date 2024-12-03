namespace TangoBotApi.Services.EventBus
{
    /// <summary>
    /// Defines a method for handling events.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Handles an event with the specified data.
        /// </summary>
        /// <param name="eventData">The data associated with the event.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task HandleEventAsync(object eventData);
    }
}

