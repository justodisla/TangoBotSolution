using System.IO;
using System.Threading.Tasks;

namespace TangoBotApi.Services.FileStorage
{
    /// <summary>
    /// Provides functionalities for file storage operations.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file to the storage.
        /// </summary>
        /// <param name="filePath">The path where the file should be stored.</param>
        /// <param name="fileStream">The stream of the file to upload.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UploadFileAsync(string filePath, Stream fileStream);

        /// <summary>
        /// Downloads a file from the storage.
        /// </summary>
        /// <param name="filePath">The path of the file to download.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the file stream.</returns>
        Task<Stream> DownloadFileAsync(string filePath);

        /// <summary>
        /// Deletes a file from the storage.
        /// </summary>
        /// <param name="filePath">The path of the file to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteFileAsync(string filePath);

        /// <summary>
        /// Checks if a file exists in the storage.
        /// </summary>
        /// <param name="filePath">The path of the file to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the file exists.</returns>
        Task<bool> FileExistsAsync(string filePath);
    }
}


