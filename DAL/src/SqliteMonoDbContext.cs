using Microsoft.EntityFrameworkCore;

namespace Mono.DAL;

public class SqliteMonoDbContext : DefaultMonoDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MyDatabase.db");
    }
}