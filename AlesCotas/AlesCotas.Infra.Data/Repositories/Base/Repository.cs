using AlesCotas.Domain.Interfaces.Repositories.Base;
using AlesCotas.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using AlesCotas.Domain.Interfaces.Base;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Domain.Entities;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AlesCotas.Infra.Data.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private ApplicationDbContext m_Context = null;
        protected ICurrentUserManager _currentUserManager;
        public ApplicationDbContext Context { get { return m_Context; } }
        DbSet<T> m_DbSet;
        public Repository(ApplicationDbContext context, ICurrentUserManager currentUserManager)
        {
            m_Context = context;
            m_DbSet = m_Context.Set<T>();
            _currentUserManager = currentUserManager;
        }
        public virtual IQueryable<T> QueryAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return m_DbSet.Where(predicate);
            }
            return m_DbSet;
        }

        //public virtual async Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities)
        //{
        //    var strategy = this.m_Context.Database.CreateExecutionStrategy();
        //    var user = await this._currentUserManager.GetCurrentUser();

        //    return await strategy.ExecuteAsync(async () =>
        //    {
        //        List<T> retorno = new List<T>(entities.Count());
        //        using (var transaction = await this.m_Context.Database.BeginTransactionAsync())
        //        {
        //            foreach (var entity in entities)
        //            {
        //                var result = this.m_Context.Set<T>().Add(entity);
        //                retorno.Add(result.Entity);
        //                this.m_Context.SaveChanges();
        //                this.m_Context.Logs.Add(new Log() { EventType = Domain.Enum.ETipoEventoLog.Insert, RefId = result.Entity.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Inserção de registro" });
        //                this.m_Context.SaveChanges();
        //            }


        //            await transaction.CommitAsync();
        //        }
        //        return retorno;
        //    });
        //}

        public virtual async Task<T> Insert(T entity, IDbContextTransaction? transacao = null)
        {
            var transaction = transacao ?? await this.m_Context.Database.BeginTransactionAsync();
            try
            {
                var user = await this._currentUserManager.GetCurrentUser();
                T retorno = null;
                var result = this.m_Context.Set<T>().Add(entity);
                retorno = result.Entity;
                this.m_Context.SaveChanges();
                this.m_Context.Logs.Add(new Log() { EventType = Domain.Enum.ETipoEventoLog.Insert, RefId = retorno.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Inserção de registro", EventDate = DateTime.Now });
                this.m_Context.SaveChanges();
                if (transacao == null)
                    await transaction.CommitAsync();
                return retorno;
            }
            catch
            {
                if (transacao == null)
                    await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transacao == null)
                    await transaction.DisposeAsync();
            }
        }



        public virtual async Task<T> Update(T entity, IDbContextTransaction? transacao = null)
        {
            var transaction = transacao ?? await this.m_Context.Database.BeginTransactionAsync();
            try
            {
                //m_DbSet.Attach(entity);
                //((IObjectContextAdapter)m_Context).ObjectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
                AttachIfNot(entity);

                var entry = m_DbSet.Update(entity);

                var user = await this._currentUserManager.GetCurrentUser();
                await this.m_Context.Logs.AddAsync(new Log() { EventType = Domain.Enum.ETipoEventoLog.Update, RefId = entity.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Alteração de registro", EventDate = DateTime.Now });

                if (transacao == null)
                    await transaction.CommitAsync();

                return entry.Entity;
            }
            catch
            {
                if (transacao == null)
                    await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transacao == null)
                    await transaction.DisposeAsync();
            }
        }

        public virtual async Task<T> Delete(T entity, IDbContextTransaction? transacao = null)
        {
            var transaction = transacao ?? await this.m_Context.Database.BeginTransactionAsync();
            try
            {
                var removeResult = this.m_Context.Remove(entity);
                var user = this._currentUserManager.GetCurrentUser();
                await this.m_Context.Logs.AddAsync(new Log() { EventType = Domain.Enum.ETipoEventoLog.Delete, RefId = entity.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Exclusão de registro", EventDate = DateTime.Now });

                if (transacao == null)
                    await transaction.CommitAsync();


                return removeResult.Entity;
            }
            catch
            {
                if (transacao == null)
                    await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transacao == null)
                    await transaction.DisposeAsync();
            }
        }

        public async Task<int> Commit()
        {
            return await this.m_Context.SaveChangesAsync();
        }

        public IQueryable<Log> Logs { get => this.Context.Logs; }

        public string GetTableName()
        {
            var models = this.m_Context.Model;
            var entityTypes = models.GetEntityTypes();
            var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
            var TableName = tableNameAnnotation.Value.ToString();
            return TableName;
        }

        protected virtual void AttachIfNot(T entity)
        {
            if (!m_DbSet.Local.Contains(entity))
            {
                m_DbSet.Attach(entity);
            }
        }

        public Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return this.m_Context.Database.BeginTransactionAsync(isolation);
        }
    }
}
