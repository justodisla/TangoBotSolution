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
    }
}
