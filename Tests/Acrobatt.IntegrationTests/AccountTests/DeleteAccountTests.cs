using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Accounts.Features.DeleteAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.AccountTests;

public class DeleteAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task DeleteConnectedAccount()
    {
        Guid accountId = Guid.NewGuid();
        using var scope = _app.Services.CreateScope();
        IAccountRepository accountRepository = (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        IAuthenticationGateway authenticationGateway = (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(accountId, "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com", "Toto!1234");
        await accountRepository.AddAsync(account, CancellationToken.None);
        authenticationGateway.Authenticate(account);
        DeleteAccountResult result = await commandDispatcher.DispatchAsync<DeleteAccount, DeleteAccountResult>(new DeleteAccount(), CancellationToken.None);
        result.AccountId.Should().Be(accountId);
    }
}