using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.Maps.Features.LoadMap;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Infrastructure.InMemory.Providers;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.UnitTests.Utils;
using NUnit.Framework;

namespace Acrobatt.UnitTests.MapsTests;

public class LoadMapTests
{
    private IStorageProvider _storageProvider = new InMemoryStorageProvider();
    private IAccountRepository _accountRepository = new InMemoryAccountRepository();
    
    [SetUp]
    public void Setup()
    {
        _storageProvider = new InMemoryStorageProvider();
        _accountRepository = AccountRepositoryFactory.GetAccountRepository();
    }

    [Test]
    public async Task LoadMapWithGameConfigurationIdWithEqualToOne()
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
        var result = await Handler().HandleAsync(Command(1), CancellationToken.None);
    }
    
    [Test]
    public async Task LoadMapWithGameConfigurationIdWithEqualToTwo()
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
        var result = await Handler().HandleAsync(Command(2), CancellationToken.None);
    }

    #region internal methods

    private LoadMapHandler Handler()
    {
        return new LoadMapHandler(_storageProvider, _accountRepository);
    }

    private LoadMapCommand Command(int gameConfigId)
    {
        return new LoadMapCommand(gameConfigId);
    }

    #endregion
}