using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.Core.Domain.Aggregates;
using TangoBotApi.Components.Repository;

namespace TangoBot.Core.Domain.Repositories
{
    internal class StrategyRepository : IRepository<SimpleStrategy>
    {
        public Task AddAsync(SimpleStrategy entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SimpleStrategy>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SimpleStrategy> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SimpleStrategy entity)
        {
            throw new NotImplementedException();
        }
    }
}
