using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public interface ICollection<T> where T : IEntity
    {
        Task<T> CreateAsync(T entity);
        Task<T?> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(T entity);
    }
}
