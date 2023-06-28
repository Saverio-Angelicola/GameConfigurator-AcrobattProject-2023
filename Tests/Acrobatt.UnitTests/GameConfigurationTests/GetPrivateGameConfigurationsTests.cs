using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.GameConfigurationTests;

public sealed class GetPrivateGameConfigurationsTests
{
    private IGameConfigurationReadRepository _gameConfigurationReadRepository = new InMemoryGameConfigurationReadRepository();
    private IAuthenticationGateway _authenticationGateway = new InMemoryAuthenticationGateway();

    [SetUp]
    public void Setup()
    {
        _gameConfigurationReadRepository = new InMemoryGameConfigurationReadRepository();
        _authenticationGateway = AuthenticationGatewayFactory.GetAuthenticationGateway();
    }

    [Test]
    public async Task ShouldBeGetPrivateGameConfigurationsWithUser1()
    {
        Account account = new(Guid.Empty, "user", "1", "User1", "user1@user.com", Password.Init("Toto!1234"));
        _authenticationGateway.Authenticate(account);
        GetPrivateGameConfigurationsQuery query = new();
        List<PrivateGameConfigurationViewModel> result = await Handler().HandleAsync(query, CancellationToken.None);
        result.Should().BeEquivalentTo(Expected());
    }
    
    [Test]
    public async Task ShouldBeGetPrivateGameConfigurationsWithUser2()
    {
        Account account = new(Guid.Empty, "user", "2", "User2", "user2@user.com", Password.Init("Toto!1234"));
        _authenticationGateway.Authenticate(account);
        GetPrivateGameConfigurationsQuery query = new();
        List<PrivateGameConfigurationViewModel> result = await Handler().HandleAsync(query, CancellationToken.None);
        result.Should().BeEquivalentTo(Expected());
    }

    #region internal methods

    private GetPrivateGameConfigurationsHandler Handler()
    {
        return new GetPrivateGameConfigurationsHandler(_authenticationGateway, _gameConfigurationReadRepository);
    }

    private List<PrivateGameConfigurationViewModel> Expected()
    {
        return new List<PrivateGameConfigurationViewModel>
        {
            new(2,"toto", 2, true)
        };
    }

    #endregion
}