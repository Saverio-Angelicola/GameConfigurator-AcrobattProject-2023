using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.AccountTests;

public class UpdateAccountTests
{
    private IAccountRepository _accountRepository = AccountRepositoryFactory.GetAccountRepository();
    private IAuthenticationGateway _authenticationGateway = AuthenticationGatewayFactory.GetAuthenticationGateway();

    [SetUp]
    public void Setup()
    {
        _accountRepository = AccountRepositoryFactory.GetAccountRepository();
        _authenticationGateway = AuthenticationGatewayFactory.GetAuthenticationGateway();
    }

    [Test]
    public async Task Should_Show_Updated_Sangel_Account()
    {
        Account account = Account.Create(Guid.NewGuid(), "Gautier", "Lassablière", "goathier", "gautier@gmail.fr", "Toto!12345");
        await AuthenticateUser(account);
        UpdateAccount updateAccount = new("sav", "ang", "sangel", "angelicola.saverio@gmail.com");
        
        UpdateAccountResult result = await Handler().HandleAsync(updateAccount, CancellationToken.None);
        
        ShouldBeEqual(result, updateAccount);
    }
    
    [Test]
    public async Task Should_Show_Updated_Fethan_Account()
    {
        Account account = Account.Create(Guid.NewGuid(), "Gautier", "Lassablière", "goathier", "gautier@gmail.fr", "Toto!12345");
        await AuthenticateUser(account);
        UpdateAccount updateAccount = new("Eth", "Fu", "Feth", "fuchs.ethan@gmail.com");
        
        UpdateAccountResult result = await Handler().HandleAsync(updateAccount, CancellationToken.None);
        
        ShouldBeEqual(result, updateAccount);
    }
    
    [Test]
    public async Task Should_Update_Account_Informations()
    {
        Account account = Account.Create(Guid.NewGuid(), "Gautier", "Lassablière", "goathier", "gautier@gmail.fr", "Toto!12345");
        await AuthenticateUser(account);
        UpdateAccount updateAccount = new("gauthier", "Lassa", "goatier", "goatier@gmail.fr");
        
        UpdateAccountResult result = await Handler().HandleAsync(updateAccount, CancellationToken.None);

        ShouldBeUpdated(account, result);
    }

    #region Internal methods

    private void ShouldBeEqual(UpdateAccountResult result, UpdateAccount updateAccount)
    {
        result.Should()
            .BeEquivalentTo(new UpdateAccountResult(updateAccount.FirstName, updateAccount.LastName, updateAccount.Pseudo, updateAccount.Email), options => options.ComparingByMembers<UpdateAccountResult>());
    }
    
    private async Task AuthenticateUser(Account account)
    {
        await _accountRepository!.AddAsync(account, CancellationToken.None);
        _authenticationGateway!.Authenticate(account);
    }

    private UpdateAccountHandler Handler()
    {
        return new UpdateAccountHandler(_accountRepository, _authenticationGateway);
    }
    
    private void ShouldBeUpdated(Account account, UpdateAccountResult result)
    {
        
        
        account.FirstName.Should().Be(result.FirstName);
        account.LastName.Should().Be(result.LastName);
        account.Pseudo.Should().Be(result.Pseudo);
        account.Email.Should().Be(result.Email);
    }

    #endregion

    
}