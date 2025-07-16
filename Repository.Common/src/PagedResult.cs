namespace Mono.Repository.Common;

public record PagedResult<TEntity>(
    List<TEntity> Items,
    int Total,
    int Page
);