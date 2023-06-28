using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Commons;

namespace Acrobatt.Domain.Accounts;

public interface IAccountRepository : IBaseRepository<Account, Guid>
{
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<GameConfiguration?> GetGameConfigurationById(int id, CancellationToken cancellationToken);
}