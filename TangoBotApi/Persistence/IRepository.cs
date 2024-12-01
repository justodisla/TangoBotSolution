using System.Collections.Generic;
using System.Threading.Tasks;

namespace TangoBotApi.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for a repository that manages entities.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity with the specified identifier, or null if no entity is found.</returns>
        Task<object> GetByIdAsync(object id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(object entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(object id);
    }
}

