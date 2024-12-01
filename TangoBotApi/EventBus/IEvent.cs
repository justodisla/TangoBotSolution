namespace TangoBotApi.EventBus
{
    /// <summary>
    /// Represents a standard event with a name and data.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the data associated with the event.
        /// </summary>
        object Data { get; }
    }
}

