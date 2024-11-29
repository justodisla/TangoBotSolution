using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TangoBot.API.Persistence.Examples
{
    public class InMemoryPersistence : IPersistence
    {
        private readonly ConcurrentDictionary<string, object> _collections = new();

        public Task<ICollection<T>> GetCollectionAsync<T>(string collectionName) where T : IEntity
        {
            if (!_collections.ContainsKey(collectionName))
            {
                throw new KeyNotFoundException($"Collection '{collectionName}' does not exist.");
            }

            var collection = _collections[collectionName] as ICollection<T>;
            if (collection == null)
            {
                throw new InvalidCastException($"Collection '{collectionName}' is not of the expected type.");
            }

            return Task.FromResult(collection);
        }

        public Task<IEnumerable<string>> ListCollectionsAsync()
        {
            var collectionNames = _collections.Keys.ToList();
            return Task.FromResult((IEnumerable<string>)collectionNames);
        }

        public Task<bool> CreateCollectionAsync<T>(string collectionName) where T : IEntity
        {
            if (_collections.ContainsKey(collectionName))
            {
                return Task.FromResult(false);
            }

            _collections[collectionName] = new Collection<T>();
            return Task.FromResult(true);
        }

        public Task<bool> RemoveCollectionAsync(string collectionName)
        {
            return Task.FromResult(_collections.TryRemove(collectionName, out _));
        }

        public void Setup(Dictionary<string, object> conf)
        {
            throw new NotImplementedException();
        }

        public bool CollectionExists(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException("Collection name cannot be null or whitespace.", nameof(collectionName));
            }

            return _collections.ContainsKey(collectionName);
        }

        private class Collection<T> : ICollection<T> where T : IEntity
        {
            private readonly ConcurrentDictionary<Guid, T> _entities = new();

            public Task<T> CreateAsync(T entity)
            {
                if (_entities.ContainsKey(entity.Id))
                {
                    throw new InvalidOperationException("Entity with the same ID already exists.");
                }

                entity.BeforeSave();
                _entities[entity.Id] = entity;
                entity.AfterSave();
                return Task.FromResult(entity);
            }

            public Task<T?> ReadAsync(Guid id)
            {
                _entities.TryGetValue(id, out var entity);
                return Task.FromResult(entity);
            }

            public Task<IEnumerable<T>> ReadAllAsync()
            {
                var entities = _entities.Values.ToList();
                return Task.FromResult((IEnumerable<T>)entities);
            }

            public Task<T> UpdateAsync(T entity)
            {
                if (!_entities.ContainsKey(entity.Id))
                {
                    throw new KeyNotFoundException("Entity not found.");
                }

                entity.BeforeSave();
                _entities[entity.Id] = entity;
                entity.AfterSave();
                return Task.FromResult(entity);
            }

            public Task<bool> DeleteAsync(Guid id)
            {
                return Task.FromResult(_entities.TryRemove(id, out _));
            }

            public Task<bool> DeleteAsync(T entity)
            {
                return DeleteAsync(entity.Id);
            }
        }
    }
}
