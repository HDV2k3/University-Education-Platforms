using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;
using System.Reflection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations
{
    public class DatabaseService<T> : IDatabaseService<T> where T : BaseDbContext, IApplicationDbContext, IDisposable
    {
        // Flag: Has Dispose already been called? recheck solution?
        private bool disposed = false;
        private readonly T _dbContext;
        private DatabaseFacade _transaction;

        public DatabaseService(T dataContext)
        {
            _dbContext = dataContext;
        }

        public DatabaseFacade Database { get { return _dbContext.Database; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
            }
            disposed = true;
        }

        public T GetContext()
        {
            return _dbContext;
        }

        public string getConnectString()
        {
            return ((T)this._dbContext).Database.GetDbConnection().ConnectionString;
        }

        public DatabaseFacade GetTransaction()
        {
            if (_transaction == null)
            {
                var context = GetContext();
                _transaction = context.Database;
            }
            return _transaction;
        }

        public void BeginTransaction(System.Data.IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            GetTransaction().BeginTransaction(isolationLevel);
        }

        public async Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            await GetTransaction().BeginTransactionAsync(isolationLevel);
        }

        public void CommitTransaction()
        {
            GetTransaction().CommitTransaction();
        }

        public void RollBackTransaction()
        {
            GetTransaction().RollbackTransaction();
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task<IEnumerable<TElement>> SqlQueryAsync<TElement>(string sqlQuery, Dictionary<string, object> parameters)
        {
            var dbConnection = ((T)this._dbContext).Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                foreach (string paramterName in parameters.Keys)
                {
                    DbParameter newParameter = command.CreateParameter();
                    newParameter.ParameterName = paramterName;
                    newParameter.Value = parameters[paramterName];

                    command.Parameters.Add(newParameter);
                }

                if (dbConnection.State != ConnectionState.Open)
                {
                    await dbConnection.OpenAsync();
                }

                using (var result = await command.ExecuteReaderAsync())
                {
                    var entities = new List<TElement>();

                    while (await result.ReadAsync())
                    {
                        var obj = Activator.CreateInstance<TElement>();

                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            try
                            {
                                if (result.GetOrdinal(prop.Name) >= 0 && !object.Equals(result[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, result[prop.Name], null);
                                }
                            }
                            catch
                            {
                            }
                        }
                        entities.Add(obj);
                    }

                    return entities;
                }
            }
        }

        public async Task<DataTable> GetDataTableAsync(string sqlQuery, Dictionary<string, object> parameters)
        {
            var dt = new DataTable();
            var dbConnection = ((T)this._dbContext).Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                foreach (string paramterName in parameters.Keys)
                {
                    DbParameter newParameter = command.CreateParameter();
                    newParameter.ParameterName = paramterName;
                    if (parameters[paramterName] == null)
                    {
                        newParameter.Value = DBNull.Value;
                    }
                    else
                        newParameter.Value = parameters[paramterName];

                    command.Parameters.Add(newParameter);
                }

                if (dbConnection.State != ConnectionState.Open)
                {
                    await dbConnection.OpenAsync();
                }

                using (var result = await command.ExecuteReaderAsync())
                {
                    dt.Load(result);
                }

                return dt;
            }
        }

        public async Task<string> ExecuteScalarAsync(string sqlQuery, Dictionary<string, object> parameters)
        {
            var dbConnection = ((T)this._dbContext).Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                foreach (string paramterName in parameters.Keys)
                {
                    DbParameter newParameter = command.CreateParameter();
                    newParameter.ParameterName = paramterName;
                    newParameter.Value = parameters[paramterName];

                    command.Parameters.Add(newParameter);
                }

                if (dbConnection.State != ConnectionState.Open)
                {
                    await dbConnection.OpenAsync();
                }

                object result = await command.ExecuteScalarAsync();
                return result.ToString();
            }
        }

        public async Task<string> ExecuteScalarAsync(string sqlQuery)
        {
            var dbConnection = ((T)this._dbContext).Database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                if (dbConnection.State != ConnectionState.Open)
                {
                    await dbConnection.OpenAsync();
                }

                object result = await command.ExecuteScalarAsync();
                return result.ToString();
            }
        }

        public Dictionary<string, (int, int, int)> GetAddUpdateDeleteEntryCount()
        {
            var entities = _dbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
            Dictionary<string, (int, int, int)> dic = new Dictionary<string, (int, int, int)>();
            entities.GroupBy(x => x.Entity.GetType().Name).ToList().ForEach(group =>
            {
                var key = group.First().Entity.GetType().Name;
                var value = (
                    group.Where(x => x.State == EntityState.Added).Count(),
                    group.Where(x => x.State == EntityState.Modified).Count(),
                    group.Where(x => x.State == EntityState.Deleted).Count()
                );
                dic.Add(key, value);
            });
            return dic;
        }
        public T GetContextScoped(IServiceScope serviceScope)
        {
            if (serviceScope != null)
            {
                return serviceScope.ServiceProvider.GetRequiredService<T>();
            }

            return GetContext();
        }
    }
}
