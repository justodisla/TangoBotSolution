namespace TangoBotApi.DI
{
    public interface IInfrService
    {
        /// <summary>
        /// Returns an array of strings with services required for this service to be loaded.
        /// </summary>
        /// <returns>An array of required service names.</returns>
        string[] Requires();
    }
}
