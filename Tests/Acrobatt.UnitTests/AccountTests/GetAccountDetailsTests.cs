using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Accounts;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.AccountTests;

public class GetAccountDetailsTests
{
    private IAccountReadRepository _accountReadRepository = new InMemoryAccountReadRepository();

    [SetUp]
    public void Setup()
    {
        _accountReadRepository = new InMemoryAccountReadRepository();
    }
    
    [Test]
    public async Task Get_User1_Account_Details()
    {
        AccountDetailsViewModel accountDetails = new(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA"), "User", "un", "User1", "user1@gmail.com");

        AccountDetailsViewModel result = await Handler().HandleAsync(new AccountDetails(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA")), CancellationToken.None);

        ShouldBeEqual(result, accountDetails);
    }
    
    [Test]
    public async Task Get_User2_Account_Details()
    {
        AccountDetailsViewModel accountDetails =
            new(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DB"), "user", "deux", "User2", "user2@gmail.com");

        AccountDetailsViewModel result = await Handler().HandleAsync(new AccountDetails(Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DB")), CancellationToken.None);

        ShouldBeEqual(result, accountDetails);
    }

    #region Internal methods

    private void ShouldBeEqual(AccountDetailsViewModel result, AccountDetailsViewModel expected)
    {
        result.Should().BeEquivalentTo(expected, options =>
        {
            options.ComparingByMembers<Account>();
            return options;
        });
    }

    private AccountDetailsHandler Handler()
    {
        return new AccountDetailsHandler(_accountReadRepository);
    }

    #endregion
    
    
}