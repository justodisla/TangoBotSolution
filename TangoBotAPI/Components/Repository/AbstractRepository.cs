using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.Persistence;

namespace TangoBotApi.Components.Repository
{
    internal abstract class AbstractRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IPersistence _persistence;
        private readonly string _collectionName;
        private readonly Services.Persistence.ICollection<T> _collection;

        protected AbstractRepository(IPersistence persistence, string collectionName)
        {
            _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            _collection = _persistence.GetCollectionAsync<T>(_collectionName).Result;
        }

        public async Task AddAsync(T entity)
        {
            var collection = await _persistence.GetCollectionAsync<T>(_collectionName);
            await collection.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity = _collection.GetAsync(id).Result;
            _collection.RemoveAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.GetAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.UpdateAsync(entity);
        }
    }
}

