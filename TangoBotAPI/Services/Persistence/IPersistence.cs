using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Persistence
{
    public interface IPersistence : IInfrService
    {
        // Methods to manage collections
        Task<ICollection<TEntity>> GetCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity;
        Task CreateCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity;
        Task RemoveCollectionAsync(string collectionName);
        Task ResetCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity;
        Task<IEnumerable<string>> GetAllCollectionsAsync();
    }
}
