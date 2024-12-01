using System;

namespace TangoBotApi.Persistence
{
    /// <summary>
    /// Represents a generic entity with an identifier.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        int Id { get; set; }
    }
}
