using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acrobatt.Infrastructure.Persistence.Repositories.write;

public sealed class AccountRepository : BaseRepository<Account, Guid>, IAccountRepository
{
    public AccountRepository(PostgresContext context): base(context) { }
    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Context.Accounts.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<GameConfiguration?> GetGameConfigurationById(int id, CancellationToken cancellationToken)
    {
        return await Context.Accounts
            .AsNoTracking()
            .SelectMany(a => a.GameConfigurations)
            .FirstOrDefaultAsync(gc => gc.Id == id, cancellationToken: cancellationToken);
    }
}