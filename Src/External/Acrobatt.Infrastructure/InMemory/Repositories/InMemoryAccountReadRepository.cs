using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Accounts;

namespace Acrobatt.Infrastructure.InMemory.Repositories;

public sealed class InMemoryAccountReadRepository : IAccountReadRepository
{
    private readonly List<Account> _accounts = new()
    {
        Account.Create(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA"), "User", "un", "User1", "user1@gmail.com", "dDd!2349"),
        Account.Create(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DB"), "user", "deux", "User2", "user2@gmail.com", "ddD!1234")
    };

    public async Task<AccountDetailsViewModel> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
       Account account = _accounts.First(a => a.Id == accountId);
       return new AccountDetailsViewModel(account.Id, account.FirstName, account.LastName, account.Pseudo, account.Email);
    }
}