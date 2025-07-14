using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Mono.DAL;
using Mono.Model.Common;

namespace Mono.Repository.Common;

public abstract class DefaultRepository<T> : IRepository<T> where T : class, IBaseEntity
{
    protected IMonoDbContext DbContext { get; }
    protected DbSet<T> DbSet { get; }

    protected DefaultRepository(IMonoDbContext dbContext, Func<IMonoDbContext, DbSet<T>> dbContextMapper)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContextMapper(DbContext);
    }

    public virtual Task<int> AddAsync(T entity)
    {
        try
        {
            var dbEntityEntry = DbSet.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }

            return Task.FromResult(1);
        }
        catch (Exception e)
        {
            throw new IOException("Unable to add", e);
        }
    }

    public virtual Task<int> UpdateAsync(T entity)
    {
        try
        {
            var dbEntityEntry = DbSet.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
            return Task.FromResult(1);
        }
        catch (Exception e)
        {
            throw new IOException("Unable to update", e);
        }
    }

    public virtual Task<int> DeleteAsync(T entity)
    {
        try
        {
            var dbEntityEntry = DbSet.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }

            return Task.FromResult(1);
        }
        catch (Exception e)
        {
            throw new IOException("Unable to delete", e);
        }
    }

    public virtual Task<int> DeleteAsync(long id)
    {
        var entity = DbSet.Find(id);
        return entity == null ? Task.FromResult(1) : DeleteAsync(entity);
    }

    public Task<int> CommitAsync()
    {
        int result = 0;
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            result = DbContext.SaveChanges();
            scope.Complete();
        }

        return Task.FromResult(result);
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}