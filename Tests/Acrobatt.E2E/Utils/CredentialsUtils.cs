using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Acrobatt.E2E.Utils;

public static class CredentialsUtils
{
    public static async Task<LoginAccountResult> GetCredentials(IServiceScope scope, string email)
    {
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        await CreateAccount(scope, email);
        LoginAccount loginCommand = new(email, "Toto!1234");
        return (await commandDispatcher.DispatchAsync<LoginAccount, LoginAccountResult>(loginCommand, CancellationToken.None));
    }

    public static async Task<RegisterAccountResult> CreateAccount(IServiceScope scope, string email)
    {
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        RegisterAccount command = new("saverio", "angelicola", "sav", email, "Toto!1234");
        return await commandDispatcher.DispatchAsync<RegisterAccount, RegisterAccountResult>(command, CancellationToken.None);
    }
}