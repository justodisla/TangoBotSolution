using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.Persistence;

namespace TangoBotApi.Components.Repository
{
    /// <summary>
    /// Defines the contract for a repository that manages entities.
    /// </summary>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Gets an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity with the specified identifier, or null if no entity is found.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(Guid id);
    }
}

