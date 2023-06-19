using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;

namespace Acrobatt.Infrastructure.InMemory.Repositories;

public sealed class InMemoryGameConfigurationReadRepository : IGameConfigurationReadRepository
{
    private readonly List<GameConfiguration> _gameConfigurations = new()
    {
        new GameConfiguration(1, "toto1", 6, 12, 10000, GameMode.Flag, false,
            new Map("map1", true, new Position(2, 3)) { Id = 1 }, new List<Team>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            }),
        new GameConfiguration(2, "toto", 6, 12, 10000, GameMode.Flag, true,
            new Map("map1", false, new Position(2, 3)) { Id = 2 },
            new List<Team>
            {
                new(1, "#FFF", 3, "team1"),
                new(2, "#000", 3, "team2")
            })
    };

    public async Task<GameConfigurationDetailViewModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        GameConfiguration gameConfiguration = _gameConfigurations.First(g => g.Id == id);

        List<TeamViewModel> teams = gameConfiguration.Teams
            .Select(t => new TeamViewModel(t.Id, t.Color, t.NbPlayer, t.Name)).ToList();

        return new GameConfigurationDetailViewModel(gameConfiguration.Id, gameConfiguration.GameName,
            gameConfiguration.MaxPlayers,
            gameConfiguration.MaxFlags, gameConfiguration.Duration, gameConfiguration.GameMode,
            gameConfiguration.IsPrivate, gameConfiguration.Map.Id)
        {
            Teams = teams
        };
    }

    public async Task<List<PublicGameConfigurationViewModel>> GetAllPublicAsync(Guid accountId,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return _gameConfigurations.Where(gc => gc.IsPrivate == false).Select(gc =>
            new PublicGameConfigurationViewModel(gc.Id, gc.GameName, gc.Map.Id, gc.IsPrivate)).ToList();
    }

    public async Task<List<PrivateGameConfigurationViewModel>> GetAllByAccountAsync(Guid accountId,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return _gameConfigurations.Where(gc => gc.IsPrivate).Select(gc =>
            new PrivateGameConfigurationViewModel(gc.Id, gc.GameName, gc.Map.Id, gc.IsPrivate)).ToList();
    }
}