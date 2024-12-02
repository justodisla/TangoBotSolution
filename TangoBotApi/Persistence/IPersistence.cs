using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.DI;

namespace TangoBotApi.Persistence
{
    public interface IPersistence : IInfrService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new();
        Task<TEntity?> GetByIdAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity, new();
        Task AddAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity;
        Task UpdateAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity;
        Task DeleteAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity;
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new();
        Task AddEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity;
        Task RemoveEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity;
    }
}
