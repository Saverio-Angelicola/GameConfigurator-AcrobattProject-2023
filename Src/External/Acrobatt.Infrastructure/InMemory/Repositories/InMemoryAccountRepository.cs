using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;

namespace Acrobatt.Infrastructure.InMemory.Repositories;

public sealed class InMemoryAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts;

    public InMemoryAccountRepository()
    {
        _accounts = new List<Account>();
    }

    public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
        return this._accounts;
    }

    public async Task<Account?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
        return _accounts.FirstOrDefault(a => a.Id == id);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        _accounts.Add(account);
        await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
    }

    public async Task DeleteAsync(Account account, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        _accounts.Remove(account);
    }

    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
        return _accounts.FirstOrDefault(a => a.Email == email);
    }

    public async Task<GameConfiguration?> GetGameConfigurationById(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return _accounts.SelectMany(a => a.GameConfigurations)
            .FirstOrDefault(gc => gc.Id == id);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        Account oldAccount = _accounts.First(a => a.Id == account.Id);
        _accounts.Remove(oldAccount);
        _accounts.Add(account);
    }
}