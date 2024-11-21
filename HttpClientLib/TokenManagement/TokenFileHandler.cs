using System;
using System.IO;
using System.Threading.Tasks;
namespace HttpClientLib.TokenManagement
{
    [Obsolete("This class is obsolete, using file to persist token was changed to config")]

    /// <summary>
    /// Handles the storage and retrieval of the session token from a file.
    /// </summary>
    public class TokenFileHandler
    {
        private readonly string _tokenFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenFileHandler"/> class.
        /// </summary>
        /// <param name="tokenFilePath">The file path where the token is stored.</param>
        public TokenFileHandler(string tokenFilePath = "session_token.txt")
        {
            _tokenFilePath = tokenFilePath;
        }

        /// <summary>
        /// Saves the session token to a file asynchronously.
        /// </summary>
        /// <param name="token">The session token to save.</param>
        public async Task SaveTokenToFileAsync(string token)
        {
            try
            {
                await File.WriteAllTextAsync(_tokenFilePath, token);
                Console.WriteLine($"[Debug] Token saved to file at {_tokenFilePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to save token to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the session token from the file asynchronously.
        /// </summary>
        /// <returns>The session token if the file exists; otherwise, null.</returns>
        public async Task<string> LoadTokenFromFileAsync()
        {
            try
            {
                if (File.Exists(_tokenFilePath))
                {
                    string token = await File.ReadAllTextAsync(_tokenFilePath);
                    Console.WriteLine("[Debug] Token loaded from file.");
                    return token;
                }
                else
                {
                    Console.WriteLine("[Debug] Token file not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to load token from file: {ex.Message}");
                return null;
            }
        }
    }
}
