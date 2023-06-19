using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.DeleteAccount;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.AccountTests;

public class RegisterAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task RegisterNewAccount()
    {
        using var scope = _app.Services.CreateScope();
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        IAccountRepository accountRepository =
            (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        RegisterAccount command = new("saverio", "angelicola", "sav", "angelicola.saverio@gmail.com", "Toto!1234");
        RegisterAccountResult result = await commandDispatcher.DispatchAsync<RegisterAccount, RegisterAccountResult>(command, CancellationToken.None);
        Account? account = await accountRepository.GetByEmailAsync("angelicola.saverio@gmail.com", CancellationToken.None);
        result.AccountId.Should().NotBeEmpty();
        account.Email.Should().Be("angelicola.saverio@gmail.com");
    }
}