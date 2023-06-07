using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Data;

namespace AlesCotas.Domain.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        public Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted);
        public IQueryable<T> QueryAll(Expression<Func<T, bool>> predicate = null);
        public Task<T> Insert(T entity, IDbContextTransaction? transaction = null);
        public Task<T> Update(T entity, IDbContextTransaction? transaction = null);
        public Task<T> Delete(T entity, IDbContextTransaction? transaction = null);
        public Task<int> Commit();
        public string GetTableName();
        public IQueryable<Log> Logs { get; }
    }
}
