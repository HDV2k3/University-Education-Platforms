using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations
{
    public interface IApplicationDbContext
    {
        DbSet<T> Repository<T>() where T : class;

        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, object>>[] includes) where T : class;

        Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
    }
}
