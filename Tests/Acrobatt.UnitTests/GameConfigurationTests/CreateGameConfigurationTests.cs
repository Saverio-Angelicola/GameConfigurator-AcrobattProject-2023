using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.Exceptions;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.Gateways;
using Acrobatt.Infrastructure.InMemory.Gateways;
using Acrobatt.Infrastructure.InMemory.Providers;
using Acrobatt.Infrastructure.InMemory.Repositories;
using Acrobatt.Infrastructure.Providers;
using Acrobatt.UnitTests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Acrobatt.UnitTests.GameConfigurationTests;

public sealed class CreateGameConfigurationTests
{
    private IGuidProvider _guidProvider = new GuidProvider();
    private IAuthenticationGateway _authenticationGateway = new AuthenticationGateway();
    private IAccountRepository _accountRepository = new InMemoryAccountRepository();
    private IStorageProvider _storageProvider = new InMemoryStorageProvider();
    
    [SetUp]
    public void Setup()
    {
        _guidProvider = GuidProviderFactory.GetGuidProvider();
        _authenticationGateway = AuthenticationGatewayFactory.GetAuthenticationGateway();
        _accountRepository =  AccountRepositoryFactory.GetAccountRepository();
        _storageProvider = new InMemoryStorageProvider();

    }

    [Test]
    public async Task Show_Configuration_Id_After_Create()
    {
        Account account = Account.Create(_guidProvider.Generate(),"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        _authenticationGateway.Authenticate(account);
        await _accountRepository.AddAsync(account, CancellationToken.None);
        
        GameConfiguration gameConfig = new(default,"toto", 6, 12, 10000, GameMode.Flag, true, new Map("map1", true, new Position(2,3)),new List<Team>()
        {
            new(1, "#FFF", 3, "team1"),
            new(2, "#000", 3, "team2")
        });
        CreateGameConfigurationResult createGameConfigResult = new(gameConfig.Id);

        CreateGameConfigurationResult result = await Handler().HandleAsync(CreateGameConfigCommand(gameConfig), CancellationToken.None);

        ShouldBeEqual(result, createGameConfigResult);
    }

    [Test]
    public async Task Show_Configuration_Id_After_Create_Another_Game_Configuration()
    {
        Account account = Account.Create(_guidProvider.Generate(),"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        _authenticationGateway.Authenticate(account);
        await _accountRepository.AddAsync(account, CancellationToken.None);
        
        GameConfiguration gameConfig = new(default, "toto1", 8, 10, 5000, GameMode.Flag, false,new Map("map1", true, new Position(2,3)), new List<Team>()
        {
            new(1, "#FFF", 3, "team1"),
            new(2, "#000", 3, "team2")
        });
        
        CreateGameConfigurationResult createGameConfigResult = new(gameConfig.Id);

        CreateGameConfigurationResult result = await Handler().HandleAsync(CreateGameConfigCommand(gameConfig), CancellationToken.None);

        ShouldBeEqual(result, createGameConfigResult);
    }

    [Test]
    public async Task Save_Game_Configuration_After_Create()
    {
        Account account = Account.Create(_guidProvider.Generate(),"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        _authenticationGateway.Authenticate(account);
        await _accountRepository.AddAsync(account, CancellationToken.None);
        
        GameConfiguration gameConfig = GameConfiguration.Create("toto", 8, 10, 5000, GameMode.Flag, false,new Map("map1", false, new Position(2,3)), new List<Team>()
        {
            new("#FFF", 3, "team1"),
            new("#000", 3, "team2")
        });

        await Handler().HandleAsync(CreateGameConfigCommand(gameConfig), CancellationToken.None);

        ShouldBeSave(gameConfig);
    }

    [Test]
    public async Task Show_Error_If_Game_Configuration_Has_One_Team_With_Height_Max_Player_After_Create()
    {
        Account account = Account.Create(_guidProvider.Generate(),"saverio", "angelicola", "sangelicola","angelicola.saverio@gmail.com", "Toto!1234");
        _authenticationGateway.Authenticate(account);
        await _accountRepository.AddAsync(account, CancellationToken.None);
        
        CreateGameConfiguration createGameConfig = new(new MemoryStream(), new CreateGameConfigurationDto("toto",8, 10,
            5000, GameMode.Flag, false, new List<TeamDto>()
            {
                new("#FFF", 3, "team1")
            }, "map1", true, new MapCenterDto(2,3)));

        Assert.ThrowsAsync<NotEnoughTeamsException>(() =>
            Handler().HandleAsync(createGameConfig, CancellationToken.None));
    }

    #region internal methods

    private CreateGameConfigurationHandler Handler()
    {
        return new CreateGameConfigurationHandler(_authenticationGateway, _accountRepository, _storageProvider);
    }

    private void ShouldBeEqual(CreateGameConfigurationResult result, CreateGameConfigurationResult expected)
    {
        result.Should().BeEquivalentTo(expected,
            options => options.ComparingByMembers<CreateGameConfigurationResult>());
    }

    private void ShouldBeSave(GameConfiguration gameConfig)
    {
        GameConfiguration? result = _authenticationGateway.GetAuthenticateAccount().GameConfigurations
            .First(gc => gc.Id == gameConfig.Id);

        result.Should().BeEquivalentTo(gameConfig, options => options.ComparingByMembers<GameConfiguration>());
    }

    private CreateGameConfiguration CreateGameConfigCommand(GameConfiguration gameConfiguration)
    {
        List<TeamDto> teams = gameConfiguration.Teams.Select(t => new TeamDto(t.Color, t.NbPlayer, t.Name)).ToList();

        return new CreateGameConfiguration(new MemoryStream(),new CreateGameConfigurationDto(gameConfiguration.GameName ,gameConfiguration.MaxPlayers, gameConfiguration.MaxFlags, gameConfiguration.Duration,
            gameConfiguration.GameMode, gameConfiguration.IsPrivate, teams, "map1", false, new MapCenterDto(2, 3)));
    }

    #endregion

    
}