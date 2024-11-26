using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public class InMemoryPersistence : IPersistence
    {
        private readonly Dictionary<string, List<IEntity>> _tables = new();

        public async Task<IEntity> CreateAsync(IEntity entity)
        {
            var tableName = entity.GetTableName();
            if (!_tables.ContainsKey(tableName))
            {
                _tables[tableName] = new List<IEntity>();
            }

            _tables[tableName].Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<IEntity> ReadAsync(Guid id)
        {
            throw new NotImplementedException("Use the overload with tableName parameter.");
        }

        public async Task<IEntity> ReadAsync(Guid id, string tableName)
        {
            if (_tables.ContainsKey(tableName))
            {
                var entity = _tables[tableName].FirstOrDefault(e => e.Id == id);
                return await Task.FromResult(entity);
            }
            return await Task.FromResult<IEntity>(null);
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync()
        {
            throw new NotImplementedException("Use the overload with tableName parameter.");
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync(string tableName)
        {
            if (_tables.ContainsKey(tableName))
            {
                return await Task.FromResult(_tables[tableName].AsEnumerable());
            }
            return await Task.FromResult(Enumerable.Empty<IEntity>());
        }

        public async Task<IEntity> UpdateAsync(IEntity entity)
        {
            var tableName = entity.GetTableName();
            if (_tables.ContainsKey(tableName))
            {
                var existingEntity = _tables[tableName].FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity != null)
                {
                    _tables[tableName].Remove(existingEntity);
                    _tables[tableName].Add(entity);
                }
            }
            return await Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return true;
            //throw new NotImplementedException("Use the overload with tableName parameter.");
        }

        public async Task<bool> DeleteAsync(Guid id, string tableName)
        {
            if (_tables.ContainsKey(tableName))
            {
                var entity = _tables[tableName].FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    _tables[tableName].Remove(entity);
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RemoveTableAsync(string tableName)
        {
            if (_tables.ContainsKey(tableName))
            {
                _tables.Remove(tableName);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<string>> ListTablesAsync()
        {
            return await Task.FromResult(_tables.Keys.AsEnumerable());
        }
    }
}
