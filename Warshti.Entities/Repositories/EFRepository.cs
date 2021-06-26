using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;

namespace Warshti.Repositories
{
    public class EFRepository : IEFRepository
    {
        private readonly WScoreContext _context;

        public EFRepository(WScoreContext context)
        {
            _context = context;
        }

        #region Generic
        public async Task AddAsync(object entity)
        {
            await _context.AddAsync(entity);
        }

        public Task UpdateAsync(object entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(object entity)
        {
            _context.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
