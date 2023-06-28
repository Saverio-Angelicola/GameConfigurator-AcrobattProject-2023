using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Domain.Accounts;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.AccountTests;

public class AccountDetailsTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task ShowingUserDetails()
    {
        Guid accountId = Guid.NewGuid();
        using var scope = _app.Services.CreateScope();
        IAccountRepository accountRepository = (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IQueryDispatcher queryDispatcher = (IQueryDispatcher)scope.ServiceProvider.GetRequiredService(typeof(IQueryDispatcher));
        Account account = Account.Create(accountId, "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com", "Toto!1234");
        await accountRepository.AddAsync(account, CancellationToken.None);
        AccountDetails query = new(accountId);
        AccountDetailsViewModel result = await queryDispatcher.DispatchAsync<AccountDetails, AccountDetailsViewModel>(query, CancellationToken.None);
        result.Email.Should().Be("angelicola.saverio@gmail.com");
    }
}