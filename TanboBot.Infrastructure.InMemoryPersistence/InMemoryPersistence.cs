using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using TangoBotApi.Services.Persistence;

namespace TangoBot.Infrastructure.InMemoryPersistence
{
    
    public class InMemoryPersistence : IPersistence
    {
        private readonly ConcurrentDictionary<string, object> _collections = new();
        TangoBotApi.Services.Persistence.ICollection<IEntity> col;

        public Task<TangoBotApi.Services.Persistence.ICollection<TEntity>> GetCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity
        {
            if (_collections.TryGetValue(collectionName, out var collection))
            {
                return Task.FromResult((TangoBotApi.Services.Persistence.ICollection<TEntity>)collection);
            }
            throw new KeyNotFoundException($"Collection '{collectionName}' not found.");
        }

        public Task CreateCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity
        {
            var collection = new InMemoryCollection<TEntity>();
            if (!_collections.TryAdd(collectionName, collection))
            {
                throw new InvalidOperationException($"Collection '{collectionName}' already exists.");
            }
            return Task.CompletedTask;
        }

        public Task RemoveCollectionAsync(string collectionName)
        {
            if (!_collections.TryRemove(collectionName, out _))
            {
                throw new KeyNotFoundException($"Collection '{collectionName}' not found.");
            }
            return Task.CompletedTask;
        }

        public Task ResetCollectionAsync<TEntity>(string collectionName) where TEntity : class, IEntity
        {
            if (_collections.TryGetValue(collectionName, out var collection))
            {
                ((InMemoryCollection<TEntity>)collection).Clear();
                return Task.CompletedTask;
            }
            throw new KeyNotFoundException($"Collection '{collectionName}' not found.");
        }

        public Task<IEnumerable<string>> GetAllCollectionsAsync()
        {
            return Task.FromResult(_collections.Keys.AsEnumerable());
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }

        private class InMemoryCollection<TEntity> : TangoBotApi.Services.Persistence.ICollection<TEntity> where TEntity : class, IEntity
        {
            private readonly ConcurrentDictionary<Guid, TEntity> _entities = new();

            public Task<IEnumerable<TEntity>> GetAllAsync()
            {
                return Task.FromResult(_entities.Values.AsEnumerable());
            }

            public Task<TEntity?> GetAsync(Guid id)
            {
                _entities.TryGetValue(id, out var entity);
                return Task.FromResult(entity);
            }

            public Task AddAsync(TEntity entity)
            {
                if (!_entities.TryAdd(entity.Id, entity))
                {
                    throw new InvalidOperationException($"Entity with ID '{entity.Id}' already exists.");
                }
                return Task.CompletedTask;
            }

            public Task UpdateAsync(TEntity entity)
            {
                if (!_entities.ContainsKey(entity.Id))
                {
                    throw new KeyNotFoundException($"Entity with ID '{entity.Id}' not found.");
                }
                _entities[entity.Id] = entity;
                return Task.CompletedTask;
            }

            public Task RemoveAsync(TEntity entity)
            {
                if (!_entities.TryRemove(entity.Id, out _))
                {
                    throw new KeyNotFoundException($"Entity with ID '{entity.Id}' not found.");
                }
                return Task.CompletedTask;
            }

            public void Clear()
            {
                _entities.Clear();
            }
        }
    }
}

