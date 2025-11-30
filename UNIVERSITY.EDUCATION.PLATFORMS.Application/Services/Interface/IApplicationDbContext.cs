using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface
{
    public interface IApplicationDbContext
    {
        // Repository
        DbSet<T> Repository<T>() where T : class;

        // Queryable with soft-delete filter
        IQueryable<T> ToQueryable<T>(bool isNotTracking = true) where T : AuditableBaseEntity;


        // Read
        Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, object>>[] includes) where T : class;
        Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        // Write
        Task<T> AddAsync<T>(T entity, bool isCommit = true) where T : class;
        Task<IReadOnlyList<T>> AddRangeAsync<T>(IReadOnlyList<T> entities, bool isCommit = true) where T : class;
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities, bool isCommit = true) where T : class;

        Task<T> UpdateAsync<T>(T entity, bool isCommit = true) where T : class;
        Task<IReadOnlyList<T>> UpdateRangeAsync<T>(IReadOnlyList<T> entities, bool isCommit = true) where T : class;
        Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> entities, bool isCommit = true) where T : class;

        // Soft Delete
        Task DeleteRangeAsync<T>(Expression<Func<T, bool>> predicate, bool isCommit = true) where T : class;

        // Raw SQL
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    }
}
