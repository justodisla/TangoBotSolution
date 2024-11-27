using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public interface IPersistence
    {
        Task<IEntity> CreateAsync(IEntity entity);
        Task<IEntity?> ReadAsync(Guid id);
        Task<IEnumerable<IEntity>> ReadAllAsync();
        Task<IEntity> UpdateAsync(IEntity entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(IEntity entity);
        Task<bool> RemoveTableAsync(string tableName);
        Task<IEnumerable<string>> ListTablesAsync();
    }

    public interface IPersistence<T> : IPersistence where T : IEntity
    {
        new Task<T> CreateAsync(IEntity entity);
        new Task<T?> ReadAsync(Guid id);
        new Task<IEnumerable<T>> ReadAllAsync();
        new Task<T> UpdateAsync(IEntity entity);
        new Task<bool> DeleteAsync(Guid id);
        new Task<bool> DeleteAsync(T entity);
    }
}
