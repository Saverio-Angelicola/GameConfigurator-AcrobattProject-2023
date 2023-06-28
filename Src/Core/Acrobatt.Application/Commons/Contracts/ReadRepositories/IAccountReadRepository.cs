using Acrobatt.Application.Accounts.Features.AccountDetails;

namespace Acrobatt.Application.Commons.Contracts.ReadRepositories;

public interface IAccountReadRepository
{
    Task<AccountDetailsViewModel> GetByIdAsync(Guid accountId, CancellationToken cancellationToken);
}