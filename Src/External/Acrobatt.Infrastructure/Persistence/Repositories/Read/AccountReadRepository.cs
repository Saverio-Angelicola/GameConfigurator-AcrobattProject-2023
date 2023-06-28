using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acrobatt.Infrastructure.Persistence.Repositories.Read;

public sealed class AccountReadRepository : BaseReadRepository, IAccountReadRepository
{
    public AccountReadRepository(PostgresContext context) : base(context)
    {
    }

    public async Task<AccountDetailsViewModel> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        string query = $"SELECT account_id AS Id, \"firstName\" AS FirstName, \"lastName\" AS LastName, pseudo AS Pseudo, email AS Email FROM accounts WHERE account_id='{accountId}'";
        return (await Context.Database.SqlQueryRaw<AccountDetailsViewModel>(query).ToListAsync(cancellationToken)).First();
    }
}