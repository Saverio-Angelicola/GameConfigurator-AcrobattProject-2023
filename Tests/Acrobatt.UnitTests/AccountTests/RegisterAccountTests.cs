using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Exceptions;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.AccountTests;

public class RegisterUserTests
{

    private IAccountRepository _accountRepository = AccountRepositoryFactory.GetAccountRepository();
    private IGuidProvider _guidProvider = GuidProviderFactory.GetGuidProvider();
    private IPasswordProvider _passwordProvider = PasswordProviderFactory.GetPasswordProvider();

    [SetUp]
    public void Setup()
    {
        _accountRepository = AccountRepositoryFactory.GetAccountRepository();
        _guidProvider = GuidProviderFactory.GetGuidProvider();
        _passwordProvider = PasswordProviderFactory.GetPasswordProvider();
    }

    [Test]
    public async Task Add_User_Sangelicola_After_Register()
    {
        Account expected = Account.Create(_guidProvider.Generate(),"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        
        await Handler().HandleAsync(RegisterAccountFromAccount(expected), CancellationToken.None);
        
        await ShouldBeSave(expected);
    }

    [Test]
    public async Task Add_FEthan_User_After_Register()
    {
        Account expected = Account.Create(_guidProvider.Generate(),"ethan", "fuchs", "fethan","fuchs.ethan@gmail.com", "Toto!1234");

        await Handler().HandleAsync(RegisterAccountFromAccount(expected), CancellationToken.None);

        await ShouldBeSave(expected);
    }

    [Test]
    public async Task Secure_Password_Before_Register()
    {
        Account account = Account.Create(_guidProvider.Generate(),"ethan", "fuchs", "fethan","fuchs.ethan@gmail.com", "Toto!1234");

        await Handler().HandleAsync(RegisterAccountFromAccount(account), CancellationToken.None);

        await PasswordShouldBeHash(account);
    }
    
    [Test]
    public async Task Show_Account_Id_After_Register()
    {
        Guid expected = _guidProvider.Generate();
        Account account = Account.Create(expected,"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        
        RegisterAccountResult result = await Handler().HandleAsync(RegisterAccountFromAccount(account), CancellationToken.None);
        
        result.AccountId.Should().Be(expected);
    }

    [Test]
    public void Show_Error_When_Password_Format_Is_Not_Valid()
    {
        RegisterAccount registerAccount = new("ethan", "fuchs", "fethan", "fuchs.ethan@gmail.com", "Tot34");

        Assert.ThrowsAsync<PasswordInvalidFormatException>(() => Handler().HandleAsync(registerAccount, CancellationToken.None));
    }

    #region Internal methods

    private RegisterAccountHandler Handler()
    {
        return new RegisterAccountHandler(_accountRepository, _guidProvider, _passwordProvider);
    }

    private RegisterAccount RegisterAccountFromAccount(Account account)
    {
        return new RegisterAccount(account.FirstName, account.LastName, account.Pseudo, account.Email, account.Password.Value);
    }

    private async Task ShouldBeSave(Account expected)
    {
        Account? result = (await _accountRepository.GetAllAsync(CancellationToken.None)).FirstOrDefault();
        
        result.Should().BeEquivalentTo(expected , options =>
        {
            options.ComparingByMembers<Account>();
            options.Excluding(account => account.Password);
            return options;
        });
    }

    private async Task PasswordShouldBeHash(Account expected)
    {
        Account? result = (await _accountRepository.GetAllAsync(CancellationToken.None)).FirstOrDefault();

        result?.Password.Value.Should().Be(string.Concat(expected.Password.Value, expected.Password.Value));
    }

    #endregion
    
    
}