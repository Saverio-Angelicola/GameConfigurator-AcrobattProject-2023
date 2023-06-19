using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Exceptions;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.Infrastructure.Providers;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.AccountTests;

public class LoginAccountTests
{
    private readonly LoginAccount _login = new("angelicola.saverio@gmail.com", "Toto!1234");
    private IAccountRepository _accountRepository = AccountRepositoryFactory.GetAccountRepository();
    private IPasswordProvider _passwordProvider = PasswordProviderFactory.GetPasswordProvider();
    private IJwtProvider _jwtProvider = JwtProviderFactory.GetJwtProvider();

    [SetUp]
    public void Setup()
    {
        _accountRepository = AccountRepositoryFactory.GetAccountRepository();
        _passwordProvider = PasswordProviderFactory.GetPasswordProvider();
        _jwtProvider = JwtProviderFactory.GetJwtProvider();
    }

    [Test]
    public async Task Get_Credentials_After_Success_Login()
    {
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola",
            "angelicola.saverio@gmail.com", "Toto!1234");
        account.UpdatePassword(_passwordProvider.Hash(account, account.Password.Value));
        await _accountRepository.AddAsync(account, CancellationToken.None)!;
        string expected = _jwtProvider.CreateJwt(account.Id);

        LoginAccountResult result = await Handler()
            .HandleAsync(_login, CancellationToken.None);

        result.Token.Should().Be(expected);
    }

    [Test]
    public async Task Get_User_Informations_and_Credentials_After_Success_Login()
    {
        Guid id = Guid.NewGuid();
        Account account = Account.Create(id, "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com",
            "Toto!1234");
        account.UpdatePassword(_passwordProvider.Hash(account, account.Password.Value));
        await _accountRepository.AddAsync(account, CancellationToken.None)!;
        LoginAccountResult expected = new(_jwtProvider.CreateJwt(account.Id), id, "angelicola.saverio@gmail.com",
            "saverio",
            "angelicola", "sangelicola");

        LoginAccountResult result = await Handler()
            .HandleAsync(_login, CancellationToken.None);

        result.Should().Be(expected);
    }

    [Test]
    public void Show_Error_When_Account_Does_not_Exist()
    {
        Assert.ThrowsAsync<AccountNotFoundException>(() => Handler()
            .HandleAsync(_login, CancellationToken.None));
    }

    [Test]
    public async Task Show_Error_When_Password_Is_Invalid()
    {
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola",
            "angelicola.saverio@gmail.com", "Toto!1235");

        account.UpdatePassword(_passwordProvider.Hash(account, account.Password.Value));
        await _accountRepository.AddAsync(account, CancellationToken.None)!;

        Assert.ThrowsAsync<IncorrectPasswordException>(() => Handler()
            .HandleAsync(_login, CancellationToken.None));
    }

    #region Internal methods

    private LoginAccountHandler Handler()
    {
        return new LoginAccountHandler(_accountRepository, _passwordProvider, _jwtProvider);
    }

    #endregion
}