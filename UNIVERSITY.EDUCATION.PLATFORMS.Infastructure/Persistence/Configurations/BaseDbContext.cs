using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations
{
    public abstract class BaseDbContext : DbContext, IApplicationDbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
            var provider = Database.ProviderName ?? string.Empty;

            if (!provider.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
            {
                Database.SetCommandTimeout(ApplicationContextOptions.COMMMAND_TIMEOUT);
            }
        }


        public BaseDbContext() { }

        // Repository
        public virtual DbSet<T> Repository<T>() where T : class => Set<T>();


        #region Soft Delete Queryable
        public virtual IQueryable<T> ToQueryable<T>(bool isNotTracking = true) where T : AuditableBaseEntity
        {
            IQueryable<T> query = Set<T>();

            // Entity có thuộc tính IsDelete thì filter
            if (typeof(T).GetProperty("IsDelete") != null)
            {
                query = query.Where(e => e.IsDeleted == false);
            }

            if (isNotTracking)
                query = query.AsNoTracking();

            return query;
        }
        #endregion



        #region Raw SQL
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await Database.ExecuteSqlRawAsync(sql, parameters);
        }
        #endregion



        #region Add
        public async Task<T> AddAsync<T>(T entity, bool isCommit = true) where T : class
        {
            Entry(entity).State = EntityState.Added;
            if (isCommit) await SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> AddRangeAsync<T>(IReadOnlyList<T> entities, bool isCommit = true) where T : class
        {
            await Set<T>().AddRangeAsync(entities);
            if (isCommit) await SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities, bool isCommit = true) where T : class
        {
            await Set<T>().AddRangeAsync(entities.ToList());
            if (isCommit) await SaveChangesAsync();
            return entities;
        }
        #endregion



        #region Update
        public async Task<T> UpdateAsync<T>(T entity, bool isCommit = true) where T : class
        {
            Entry(entity).State = EntityState.Modified;
            if (isCommit) await SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> UpdateRangeAsync<T>(IReadOnlyList<T> entities, bool isCommit = true) where T : class
        {
            Set<T>().UpdateRange(entities);
            if (isCommit) await SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> entities, bool isCommit = true) where T : class
        {
            Set<T>().UpdateRange(entities.ToList());
            if (isCommit) await SaveChangesAsync();
            return entities;
        }
        #endregion



        #region Hard Delete
        public async Task HardDeleteRangeAsync<T>(Expression<Func<T, bool>> predicate, bool isCommit = true) where T : class
        {
            var entities = Set<T>().Where(predicate).ToList();
            if (entities.Any())
            {
                RemoveRange(entities);
                if (isCommit) await SaveChangesAsync();
            }
        }

        public async Task HardDeleteRangeAsync<T>(IEnumerable<T> entities, bool isCommit = true) where T : class
        {
            var list = entities.ToList();
            if (list.Any())
            {
                RemoveRange(list);
                if (isCommit) await SaveChangesAsync();
            }
        }

        public async Task HardDeleteAsync<T>(Expression<Func<T, bool>> predicate, bool isCommit = true) where T : class
        {
            var entity = await Set<T>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                Remove(entity);
                if (isCommit) await SaveChangesAsync();
            }
        }

        public async Task HardDeleteAsync<T>(T entity, bool isCommit = true) where T : class
        {
            if (entity == null) return;
            Remove(entity);
            if (isCommit) await SaveChangesAsync();
        }
        #endregion

        #region Soft Delete
        public async Task DeleteRangeAsync<T>(Expression<Func<T, bool>> predicate, bool isCommit = true) where T : class
        {
            var entities = Set<T>().Where(predicate).ToList();

            foreach (var entity in entities)
            {
                var prop = entity.GetType().GetProperty("IsDelete");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                }
            }

            if (isCommit && entities.Any())
                await SaveChangesAsync();
        }
        #endregion

        #region Query (Read)
        public async Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = Set<T>().Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await Set<T>().CountAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        #endregion
    }
}
