using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public interface IPersistence
    {
        Task<IEntity> CreateAsync(IEntity entity);
        Task<IEntity> ReadAsync(Guid id);
        Task<IEnumerable<IEntity>> ReadAllAsync();
        Task<IEntity> UpdateAsync(IEntity entity);
        Task<bool> DeleteAsync(Guid id);

        // Method to remove a table with all its records if confirmed
        Task<bool> RemoveTableAsync(string tableName);

        // Method to return a list of all tables
        Task<IEnumerable<string>> ListTablesAsync();
    }
}
