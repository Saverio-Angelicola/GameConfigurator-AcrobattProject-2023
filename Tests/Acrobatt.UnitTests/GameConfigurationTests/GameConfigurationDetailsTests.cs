using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.Infrastructure.Providers;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.GameConfigurationTests;

public sealed class GameConfigurationDetailsTests
{
    private IGuidProvider _guidProvider = new GuidProvider();

    [SetUp]
    public void Setup()
    {
        _guidProvider = GuidProviderFactory.GetGuidProvider();
    }

    [Test]
    public async Task ShouldShowPublicGameConfigurationDetail()
    {
        Guid id = _guidProvider.Generate();
        IGameConfigurationReadRepository gameConfigurationReadRepository = new InMemoryGameConfigurationReadRepository();

        GameConfigurationDetailViewModel gameConfigurationViewModel = new(1,"toto1", 6, 12, 10000, GameMode.Flag, false, 1)
        {
            Teams = new List<TeamViewModel>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            }
        };
        
        GameConfigurationDetail query = new(1);

        GameConfigurationDetailHandler handler = new(gameConfigurationReadRepository);

        GameConfigurationDetailViewModel result = await handler.HandleAsync(query, CancellationToken.None);

        result.Should().BeEquivalentTo(gameConfigurationViewModel, options => options.ComparingByMembers<PrivateGameConfigurationViewModel>());
    }
    
    [Test]
    public async Task ShouldShowPrivateGameConfigurationDetail()
    {
        Guid id = _guidProvider.Generate();
        IGameConfigurationReadRepository gameConfigurationReadRepository = new InMemoryGameConfigurationReadRepository();

        GameConfigurationDetailViewModel gameConfigurationViewModel = new(2,"toto", 6, 12, 10000, GameMode.Flag, true, 2)
        {
            Teams = new List<TeamViewModel>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            }
        };

        GameConfigurationDetailHandler handler = new(gameConfigurationReadRepository);

        GameConfigurationDetail query = new(2);

        GameConfigurationDetailViewModel result = await handler.HandleAsync(query, CancellationToken.None);

        result.Should().BeEquivalentTo(gameConfigurationViewModel, options => options.ComparingByMembers<PrivateGameConfigurationViewModel>());
    }
}