namespace TangoBotApi.EventBus
{
    /// <summary>
    /// Represents a standard event with a name and data.
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="data">The data associated with the event.</param>
        public Event(string name, object data)
        {
            Name = name;
            Data = data;
        }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the data associated with the event.
        /// </summary>
        public object Data { get; }
    }
}

