using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TangoBotAPI.Persistence
{
    public class InMemoryPersistence : IPersistence
    {
        private readonly List<IEntity> _entities = new();

        public async Task<IEntity> CreateAsync(IEntity entity)
        {
            _entities.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<IEntity> ReadAsync(Guid id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<IEntity>> ReadAllAsync()
        {
            return await Task.FromResult(_entities.AsEnumerable());
        }

        public async Task<IEntity> UpdateAsync(IEntity entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
                _entities.Add(entity);
            }
            return await Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _entities.Remove(entity);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
