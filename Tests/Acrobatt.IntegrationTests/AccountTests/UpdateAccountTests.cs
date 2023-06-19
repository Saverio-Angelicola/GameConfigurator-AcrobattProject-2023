using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.AccountTests;

public class UpdateAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task UpdateAccountWithNewEmail()
    {
        using var scope = _app.Services.CreateScope();
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        IAccountRepository accountRepository = (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IAuthenticationGateway authenticationGateway = (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com", "Toto!1234");
        UpdateAccount command = new("saverio", "angelicola", "sav", "toto@gmail.com");
        await accountRepository.AddAsync(account, CancellationToken.None);
        authenticationGateway.Authenticate(account);
        UpdateAccountResult result = await commandDispatcher.DispatchAsync<UpdateAccount, UpdateAccountResult>(command, CancellationToken.None);
        result.Email.Should().Be("toto@gmail.com");
    }
}