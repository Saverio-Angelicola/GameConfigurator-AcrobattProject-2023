using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.DeleteAccount;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.AccountTests;

public class DeleteAccountTests
{
    private IAccountRepository _accountRepository = new InMemoryAccountRepository();
    private IAuthenticationGateway _authenticationGateway = new InMemoryAuthenticationGateway();

    [SetUp]
    public void Setup()
    {
        _accountRepository = AccountRepositoryFactory.GetAccountRepository();
        _authenticationGateway = AuthenticationGatewayFactory.GetAuthenticationGateway();
    }
    
    [Test]
    public async Task ShowDeletedAccountId()
    {
        Guid id = Guid.NewGuid();

        _authenticationGateway.Authenticate(DeletedAccount(id));

        DeleteAccountResult result = await Handler().HandleAsync(new DeleteAccount(), CancellationToken.None);

        result.AccountId.Should().Be(id);
    }

    [Test]
    public async Task ShouldDeleteUser()
    {
        Guid id = Guid.NewGuid();

        _authenticationGateway.Authenticate(DeletedAccount(id));

        await Handler().HandleAsync(new DeleteAccount(), CancellationToken.None);

        (await _accountRepository.GetAsync(id, CancellationToken.None)).Should().BeNull();
    }

    #region Internal methods

    private DeleteAccountHandler Handler()
    {
        return new DeleteAccountHandler(_accountRepository, _authenticationGateway);
    }

    private Account DeletedAccount(Guid id)
    {
        return Account.Create(id, "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com", "Toto!1234");
    }

    #endregion

    
}