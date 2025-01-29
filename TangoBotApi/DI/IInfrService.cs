namespace TangoBotApi.Services.DI
{
    /// <summary>
    /// Defines the interface for infrastructure services in the TangoBot application.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are expected to provide information about their dependencies and setup configuration.
    /// </remarks>
    public interface IInfrService
    {
        /// <summary>
        /// Returns an array of strings with services required for this service to be loaded.
        /// </summary>
        /// <returns>An array of required service names.</returns>
        /// <remarks>
        /// This method should return the fully qualified names of the services that this service depends on.
        /// If there are no dependencies, an empty array should be returned.
        /// </remarks>
        /// <example>
        /// <code>
        /// public string[] Requires()
        /// {
        ///     return new string[] { "Namespace.ServiceA", "Namespace.ServiceB" };
        /// }
        /// </code>
        /// </example>
        string[] Requires();

        /// <summary>
        /// Sets up the service with the provided configuration.
        /// </summary>
        /// <param name="configuration">A dictionary containing configuration settings.</param>
        /// <remarks>
        /// This method is used to configure the service with the necessary settings.
        /// The configuration dictionary can contain various settings required by the service.
        /// </remarks>
        /// <example>
        /// <code>
        /// public void Setup(Dictionary<string, object> configuration)
        /// {
        ///     if (configuration.TryGetValue("SettingKey", out var value))
        ///     {
        ///         // Use the value to configure the service
        ///     }
        /// }
        /// </code>
        /// </example>
        void Setup(Dictionary<string, object> configuration);
    }
}

