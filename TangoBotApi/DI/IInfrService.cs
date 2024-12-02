namespace TangoBotApi.DI
{
    public interface IInfrService
    {
        /// <summary>
        /// Returns an array of strings with services required for this service to be loaded.
        /// </summary>
        /// <returns>An array of required service names.</returns>
        string[] Requires();

        /// <summary>
        /// Sets up the service with the provided configuration.
        /// </summary>
        /// <param name="configuration">A dictionary containing configuration settings.</param>
        void Setup(Dictionary<string, object> configuration);
    }
}
