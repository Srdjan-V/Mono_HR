using Microsoft.EntityFrameworkCore;

namespace Mono.DAL;

public class InMemorySqliteMonoDbContext : DefaultMonoDbContext
{
    public InMemorySqliteMonoDbContext()
    {
        Database.OpenConnection();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
    }

    public override void Dispose()
    {
        //this class should be a singleton shared amongs all tests in a contexts
        /*Database.CloseConnection();
        base.Dispose();*/
    }
}