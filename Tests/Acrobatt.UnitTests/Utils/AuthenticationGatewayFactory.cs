using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;

namespace Acrobatt.UnitTests.Utils;

public class AuthenticationGatewayFactory
{
    public static IAuthenticationGateway GetAuthenticationGateway()
    {
        return new InMemoryAuthenticationGateway();
    }
}