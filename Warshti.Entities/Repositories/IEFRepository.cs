using System.Threading.Tasks;

namespace Warshti.Repositories
{
    public interface IEFRepository
    {
        Task AddAsync(object entity);
        Task DeleteAsync(object entity);
        Task<bool> SaveChangesAsync();
        Task UpdateAsync(object entity);
    }
}