namespace Mono.DAL;

public interface IMonoDbContextFactory
{
    public IMonoDbContext CreateDbContext();
}