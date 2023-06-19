using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Infrastructure.InMemory.Gateways;

public sealed class InMemoryAuthenticationGateway : IAuthenticationGateway
{
    private Account? _account;
    
    public void Authenticate(Account account)
    {
        _account = account;
    }

    public void Authenticate(Account account, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Account GetAuthenticateAccount()
    {
        return _account ?? throw new NullReferenceException();
    }
}