using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    /// <summary>
    /// Defines the contract for persistence operations for entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity that implements <see cref="IEntity"/>.</typeparam>
    public interface IPersistence<T> where T : IEntity
    {
        /// <summary>
        /// Asynchronously creates a new entity in the persistence store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity.</returns>
        Task<T> CreateAsync(IEntity entity);

        /// <summary>
        /// Asynchronously reads an entity from the persistence store by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<T> ReadAsync(Guid id);

        /// <summary>
        /// Asynchronously reads all entities from the persistence store.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> ReadAllAsync();

        /// <summary>
        /// Asynchronously updates an existing entity in the persistence store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        Task<T> UpdateAsync(IEntity entity);

        /// <summary>
        /// Asynchronously deletes an entity from the persistence store by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>True if the entity was deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Asynchronously deletes an entity from the persistence store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>True if the entity was deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Asynchronously removes a table and all its records from the persistence store if confirmed.
        /// </summary>
        /// <param name="tableName">The name of the table to remove.</param>
        /// <returns>True if the table was removed; otherwise, false.</returns>
        Task<bool> RemoveTableAsync(string tableName);

        /// <summary>
        /// Asynchronously returns a list of all tables in the persistence store.
        /// </summary>
        /// <returns>A collection of table names.</returns>
        Task<IEnumerable<string>> ListTablesAsync();
    }
}
