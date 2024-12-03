using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotApi.Services.Persistence
{
    /// <summary>
    /// Provides methods for asynchronous operations on collections of entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface ICollection<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets all entities in the collection asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets a single entity by its Guid ID asynchronously.
        /// </summary>
        /// <param name="id">The Guid ID of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// Adds a new entity to the collection asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity in the collection asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Removes an entity from the collection asynchronously.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(TEntity entity);
    }
}

