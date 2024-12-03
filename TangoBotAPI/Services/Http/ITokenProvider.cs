using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Http
{
    /// <summary>
    /// Interface for providing tokens.
    /// </summary>
    public interface ITokenProvider : IInfrService
    {
        /// <summary>
        /// Gets a valid token asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the valid token.</returns>
        Task<string?> GetValidTokenAsync();

        /// <summary>
        /// Gets a valid streaming token asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the valid streaming token.</returns>
        Task<string> GetValidStreamingToken();
    }
}