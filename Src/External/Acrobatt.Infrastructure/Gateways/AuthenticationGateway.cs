using System.Security.Claims;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Acrobatt.Infrastructure.Gateways;

public class AuthenticationGateway : IAuthenticationGateway
{
    private Account? _account;
    
    public void Authenticate(Account account)
    {
        _account = account;
    }

    public Account GetAuthenticateAccount()
    {
        if (_account is null)
        {
            throw new UnauthorizedAccessException();
        }
        return _account;
    }

    public static AuthenticationGateway AddGateway(IServiceProvider options)
    {
        string? userId = options.GetService<IHttpContextAccessor>()
            ?.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
            
        AuthenticationGateway authenticationGateway = new();

        if (string.IsNullOrEmpty(userId))
        {
            return authenticationGateway;
        }
        
        Account? account = options.GetService<IAccountRepository>()?.GetAsync(Guid.Parse(userId), new CancellationToken()).Result;
        if (account != null)
        {
            authenticationGateway.Authenticate(account);
        }

        return authenticationGateway;
    }
}