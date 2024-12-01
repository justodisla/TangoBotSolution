using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotApi.Persistence
{
    /// <summary>
    /// Provides methods for asynchronous CRUD operations on entities within collections.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IPersistence<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets all entities in the specified collection asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(string collectionName);

        /// <summary>
        /// Gets an entity by its identifier in the specified collection asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        Task<TEntity?> GetByIdAsync(string collectionName, int id);

        /// <summary>
        /// Adds a new entity to the specified collection asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(string collectionName, TEntity entity);

        /// <summary>
        /// Updates an existing entity in the specified collection asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(string collectionName, TEntity entity);

        /// <summary>
        /// Deletes an entity by its identifier in the specified collection asynchronously.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(string collectionName, int id);
    }
}
