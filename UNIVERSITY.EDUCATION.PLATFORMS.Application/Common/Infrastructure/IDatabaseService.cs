using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Paged;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure
{
    public interface IDatabaseService<T> where T : DbContext, IApplicationDbContext, IDisposable
    {
        DatabaseFacade Database { get; }

        void Dispose();
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        Task<int> ExecuteSqlRawAsync(string sql);

        Task<IEnumerable<TElement>> SqlQueryAsync<TElement>(string sqlQuery, Dictionary<string, object> parameters);

        Task<DataTable> GetDataTableAsync(string sqlQuery, Dictionary<string, object> parameters);

        Task<string> ExecuteScalarAsync(string sqlQuery, Dictionary<string, object> parameters);

        Task<string> ExecuteScalarAsync(string sqlQuery);

        T GetContext();
        string getConnectString();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable);
        void CommitTransaction();
        void RollBackTransaction();

        Dictionary<string, (int, int, int)> GetAddUpdateDeleteEntryCount();
    
        T GetContextScoped(IServiceScope serviceScope);
        Task<PagedResponse<IEnumerable<TEntity>>> GetPagedReponseAsync<TEntity>(IQueryable<TEntity> queryable, SmartTableParam param) where TEntity : class;

    }
}
