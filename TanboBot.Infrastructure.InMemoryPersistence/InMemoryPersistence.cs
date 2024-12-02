using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangoBotApi.Persistence;

namespace TangoBotApi.Infrastructure
{
    public class InMemoryPersistence : IPersistence
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, IEntity>> _store = new();

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new()
        {
            if (_store.TryGetValue(collectionName, out var collection))
            {
                return Task.FromResult(collection.Values.OfType<TEntity>());
            }

            return Task.FromResult(Enumerable.Empty<TEntity>());
        }

        public Task<TEntity?> GetByIdAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity, new()
        {
            if (_store.TryGetValue(collectionName, out var collection) && collection.TryGetValue(id, out var entity))
            {
                return Task.FromResult(entity as TEntity);
            }

            return Task.FromResult<TEntity?>(null);
        }

        public Task AddAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            var collection = _store.GetOrAdd(collectionName, new ConcurrentDictionary<int, IEntity>());
            collection[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task UpdateAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            if (_store.TryGetValue(collectionName, out var collection))
            {
                collection[entity.Id] = entity;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync<TEntity>(string collectionName, int id) where TEntity : class, IEntity
        {
            if (_store.TryGetValue(collectionName, out var collection))
            {
                collection.TryRemove(id, out _);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TEntity>> GetAllEntitiesAsync<TEntity>(string collectionName) where TEntity : class, IEntity, new()
        {
            return GetAllAsync<TEntity>(collectionName);
        }

        public Task AddEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            return AddAsync(collectionName, entity);
        }

        public Task RemoveEntityAsync<TEntity>(string collectionName, TEntity entity) where TEntity : class, IEntity
        {
            return DeleteAsync<TEntity>(collectionName, entity.Id);
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
