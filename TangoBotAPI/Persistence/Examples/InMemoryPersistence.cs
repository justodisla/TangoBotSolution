using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public class InMemoryPersistence : IPersistence
    {
        private readonly Dictionary<Guid, Table> _tables = new();

        public async Task<IEntity> CreateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            var table = GetOrCreateTable(tableName);

            table.Entities.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<IEntity?> ReadAsync(Guid id)
        {
            foreach (var table in _tables.Values)
            {
                var entity = table.Entities.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    return await Task.FromResult(entity);
                }
            }

            return await Task.FromResult<IEntity?>(null);
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync()
        {
            var allEntities = _tables.Values.SelectMany(t => t.Entities).ToList();
            return await Task.FromResult(allEntities);
        }

        public async Task<IEntity> UpdateAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            var table = GetOrCreateTable(tableName);

            var existingEntity = table.Entities.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                table.Entities.Remove(existingEntity);
                table.Entities.Add(entity);
            }
            return await Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            foreach (var table in _tables.Values)
            {
                var entity = table.Entities.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    table.Entities.Remove(entity);
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(IEntity entity)
        {
            var tableName = entity.GetEntityName();
            if (_tables.Values.Any(t => t.Name == tableName))
            {
                var table = _tables.Values.First(t => t.Name == tableName);
                if (table.Entities.Remove(entity))
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RemoveTableAsync(string tableName)
        {
            var table = _tables.Values.FirstOrDefault(t => t.Name == tableName);
            if (table != null)
            {
                _tables.Remove(table.Id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<string>> ListTablesAsync()
        {
            var tableNames = _tables.Values.Select(t => t.Name).ToList();
            return await Task.FromResult(tableNames);
        }

        private Table GetOrCreateTable(string tableName)
        {
            var table = _tables.Values.FirstOrDefault(t => t.Name == tableName);
            if (table == null)
            {
                table = new Table
                {
                    Id = Guid.NewGuid(),
                    Name = tableName,
                    Description = $"Description for {tableName}",
                    Entities = new List<IEntity>()
                };
                _tables[table.Id] = table;
            }
            return table;
        }

        private class Table
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public List<IEntity> Entities { get; set; } = new List<IEntity>();
        }
    }

    public class InMemoryPersistence<T> : InMemoryPersistence, IPersistence<T> where T : class, IEntity
    {
        public new async Task<T> CreateAsync(IEntity entity)
        {
            return (T)await base.CreateAsync(entity);
        }

        public new async Task<T?> ReadAsync(Guid id)
        {
            return (T?)await base.ReadAsync(id);
        }

        public new async Task<IEnumerable<T>> ReadAllAsync()
        {
            return (IEnumerable<T>)await base.ReadAllAsync();
        }

        public new async Task<T> UpdateAsync(IEntity entity)
        {
            return (T)await base.UpdateAsync(entity);
        }

        public new async Task<bool> DeleteAsync(Guid id)
        {
            return await base.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await base.DeleteAsync((IEntity)entity);
        }
    }
}
