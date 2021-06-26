using System.Threading.Tasks;

namespace WScore.Data.Repositories
{
    public interface IEFRepository
    {
        Task AddAsync(object entity);
        Task DeleteAsync(object entity);
        Task<bool> SaveChangesAsync();
        Task UpdateAsync(object entity);
    }
}