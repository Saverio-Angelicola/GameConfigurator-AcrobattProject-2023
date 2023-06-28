using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;

namespace Acrobatt.Application.Accounts.Features.AccountDetails;

public sealed class AccountDetailsHandler : IQueryHandler<AccountDetails, AccountDetailsViewModel>
{
    private readonly IAccountReadRepository _accountReadRepository;
    
    public AccountDetailsHandler(IAccountReadRepository accountReadRepository)
    {
        _accountReadRepository = accountReadRepository;
    }

    public async Task<AccountDetailsViewModel> HandleAsync(AccountDetails getAccountDetails, CancellationToken cancellationToken)
    {
        return await _accountReadRepository.GetByIdAsync(getAccountDetails.AccountId, cancellationToken);
    }
}