using System.Threading.Tasks;

namespace TangoBotApi.Infrastructure
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


