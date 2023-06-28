using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.GameConfigurationTests;

public sealed class GetPublicGameConfigurationsTests
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
    public async Task ShouldBeGetPublicGameConfigurationsWithUser1()
    {
        Account account = new(Guid.Empty, "user", "1", "User1", "user1@user.com", Password.Init("Toto!1234"));
        _authenticationGateway.Authenticate(account);
        GetPublicGameConfigurationsQuery query = new();
        List<PublicGameConfigurationViewModel> result = await Handler().HandleAsync(query, CancellationToken.None);
        result.Should().BeEquivalentTo(Expected());
    }
    
    [Test]
    public async Task ShouldBeGetPublicGameConfigurationsWithUser2()
    {
        Account account = new(Guid.Empty, "user", "2", "User2", "user2@user.com", Password.Init("Toto!1234"));
        _authenticationGateway.Authenticate(account);
        GetPublicGameConfigurationsQuery query = new();
        List<PublicGameConfigurationViewModel> result = await Handler().HandleAsync(query, CancellationToken.None);
        result.Should().BeEquivalentTo(Expected());
    }

    #region internal methods

    private GetPublicGameConfigurationsHandler Handler()
    {
        return new GetPublicGameConfigurationsHandler(_authenticationGateway, _gameConfigurationReadRepository);
    }

    private List<PublicGameConfigurationViewModel> Expected()
    {
        return new List<PublicGameConfigurationViewModel>
        {
            new(1,"toto1", 1, false)
        };
    }

    #endregion
}