using System.Collections.Generic;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.EventBus
{
    /// <summary>
    /// Provides methods for subscribing to and publishing events.
    /// </summary>
    public interface IEventBus : IInfrService
    {
        /// <summary>
        /// Subscribes to a specific named event.
        /// </summary>
        /// <param name="eventName">The name of the event to subscribe to.</param>
        /// <param name="listener">The event listener that will handle the event.</param>
        void Subscribe(string eventName, IEventListener listener);

        /// <summary>
        /// Unsubscribes from a specific named event.
        /// </summary>
        /// <param name="eventName">The name of the event to unsubscribe from.</param>
        /// <param name="listener">The event listener to remove.</param>
        void Unsubscribe(string eventName, IEventListener listener);

        /// <summary>
        /// Publishes an event to all subscribed listeners.
        /// </summary>
        /// <param name="eventName">The name of the event to publish.</param>
        /// <param name="eventData">The data associated with the event.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task PublishAsync(string eventName, object eventData);

        /// <summary>
        /// Retrieves all listeners subscribed to a specific named event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>A list of event listeners subscribed to the event.</returns>
        IList<IEventListener> GetListeners(string eventName);
    }
}
