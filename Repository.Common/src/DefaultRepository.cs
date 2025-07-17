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

    public ValueTask<PagedResult<T>> FindPaged(int page, int pageSize, Func<T, bool>? filter,
        IComparer<T>? comparer)
    {
        IEnumerable<T> query = DbSet.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (comparer != null)
        {
            query = query.Order(comparer);
        }

        var baseEntities = query.ToList();
        var totalCount = baseEntities.Count();
        var items = baseEntities
            .Skip(Math.Max(0, page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<T>(items, totalCount, page);
        return ValueTask.FromResult(result);
    }

    public virtual Task<List<T>> FindAll()
    {
        return DbSet.AsNoTracking().ToListAsync();
    }

    public virtual Task<int> CountAsync()
    {
        return DbSet.CountAsync();
    }

    public virtual ValueTask<T?> GetAsync(long id)
    {
        try
        {
            return DbSet.FindAsync(id);
        }
        catch (Exception e)
        {
            throw new IOException("Unable to find", e);
        }
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
            var trackedEntity = DbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (trackedEntity != null)
            {
                DbSet.Entry(trackedEntity).State = EntityState.Detached;
            }

            DbSet.Attach(entity);
            DbSet.Entry(entity).State = EntityState.Modified;
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

    public async Task<int> CommitAsync()
    {
        return await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}