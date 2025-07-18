﻿using Mono.Model.Common;

namespace Mono.Repository.Common;

public interface IRepository<T> : IDisposable where T : IBaseEntity
{
    Task<int> CommitAsync();

    ValueTask<PagedResult<T>> FindPaged(int page, int pageSize, Func<T, bool>? filter,
        IComparer<T>? comparer);

    Task<List<T>> FindAll();

    Task<int> CountAsync();

    //UnitOfWork
    ValueTask<T?> GetAsync(long id);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<int> DeleteAsync(long id);
}