using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.GameConfigurations.Features.GenerateGame;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Infrastructure.InMemory.Providers;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.Infrastructure.Providers;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.GameConfigurationTests;

public sealed class GenerateGameTests
{
    private IGameGeneratorProvider _gameGeneratorProvider = new InMemoryGameGeneratorProvider();
    private IAccountRepository _accountRepository = new InMemoryAccountRepository();

    [SetUp]
    public void SetUp()
    {
        _gameGeneratorProvider = new InMemoryGameGeneratorProvider();
        _accountRepository = new InMemoryAccountRepository();
    }

    [Test]
    public async Task ShouldBeGenerateGameForGameConfigurationWithIdEqualToOne()
    {
        Account account = new(Guid.Empty, "user", "2", "User2", "user2@user.com", Password.Init("Toto!1234"));
        account.AddGameConfiguration(new GameConfiguration(1, "ma partie 1", 6, 12, 10000, GameMode.Flag, false,
            new Map("map1", false, new Position(2, 3)) { Id = 2 },
            new List<Team>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            }));
        
        await _accountRepository.AddAsync(account, CancellationToken.None);
        GenerateGameResult result = await Handler().HandleAsync(Command(1), CancellationToken.None);
        result.Should().BeEquivalentTo(new GenerateGameResult(Stream.Null),options => options.ComparingByMembers<GenerateGameResult>());
    }
    
    [Test]
    public async Task ShouldBeGenerateGameForGameConfigurationWithIdEqualToTwo()
    {
        Account account = new(Guid.Empty, "user", "2", "User2", "user2@user.com", Password.Init("Toto!1234"));
        account.AddGameConfiguration(new GameConfiguration(2, "ma partie 2", 6, 12, 10000, GameMode.Supremacy, true,
            new Map("map1", false, new Position(2, 3)) { Id = 2 },
            new List<Team>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            }));
        
        await _accountRepository.AddAsync(account, CancellationToken.None);
        GenerateGameResult result = await Handler().HandleAsync(Command(1), CancellationToken.None);
        result.Should().BeEquivalentTo(new GenerateGameResult(Stream.Null));
    }

    #region internal methods

    private GenerateGameHandler Handler()
    {
        return new GenerateGameHandler(_gameGeneratorProvider, _accountRepository);
    }

    private GenerateGameCommand Command(int gameConfigId)
    {
        return new GenerateGameCommand(gameConfigId);
    }

    #endregion
}