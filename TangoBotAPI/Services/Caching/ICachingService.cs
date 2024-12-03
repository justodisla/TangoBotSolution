using System.Threading.Tasks;

namespace TangoBotApi.Services.Caching
{
    /// <summary>
    /// Provides caching functionalities.
    /// </summary>
    public interface ICachingService
    {
        Task SetAsync(string key, object value);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}


