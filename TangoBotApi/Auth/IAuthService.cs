using System.Threading.Tasks;

namespace TangoBotApi.Auth
{
    /// <summary>
    /// Provides functionalities for authentication and authorization.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the authentication was successful.</returns>
        Task<bool> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Generates a token for the authenticated user.
        /// </summary>
        /// <param name="username">The username of the authenticated user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated token.</returns>
        Task<string> GenerateTokenAsync(string username);

        /// <summary>
        /// Authorizes a user based on the provided token and required role.
        /// </summary>
        /// <param name="token">The token of the user.</param>
        /// <param name="requiredRole">The required role for authorization.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the authorization was successful.</returns>
        Task<bool> AuthorizeAsync(string token, string requiredRole);
    }
}



