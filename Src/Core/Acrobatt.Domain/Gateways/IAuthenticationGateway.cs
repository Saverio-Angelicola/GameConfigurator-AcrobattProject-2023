using Acrobatt.Domain.Accounts;

namespace Acrobatt.Domain.Gateways;

public interface IAuthenticationGateway
{
    void Authenticate(Account account);

    Account GetAuthenticateAccount();
}