using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;

namespace Acrobatt.Infrastructure.Persistence.Contexts;

public class PostgresContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<GameConfiguration> GameConfigurations { get; set; }

    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfConfiguration).Assembly);
    }
}