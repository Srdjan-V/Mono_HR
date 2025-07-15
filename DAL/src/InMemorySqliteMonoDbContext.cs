using Microsoft.EntityFrameworkCore;

namespace Mono.DAL;

public class InMemorySqliteMonoDbContext : DefaultMonoDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
    }
}