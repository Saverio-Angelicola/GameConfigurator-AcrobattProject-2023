using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acrobatt.Infrastructure.Persistence.Repositories.Read;

public sealed class GameConfigurationReadRepository : BaseReadRepository, IGameConfigurationReadRepository
{
    public GameConfigurationReadRepository(PostgresContext context) : base(context)
    {
    }

    public async Task<GameConfigurationDetailViewModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        string query = $"SELECT gc.game_configuration_id as Id, gc.max_players AS MaxPlayers, gc.max_flags AS MaxFlags, gc.duration AS Duration, gc.\"gameMode\" AS GameMode, gc.is_private AS IsPrivate, gc.game_name AS GameName, m.map_id AS MapId FROM game_configurations gc INNER JOIN maps m on gc.game_configuration_id = m.map_id and gc.game_configuration_id = {id}";

        GameConfigurationDetailViewModel gameConfiguration = (await Context.Database
            .SqlQueryRaw<GameConfigurationDetailViewModel>(query)
            .ToListAsync(cancellationToken))
            .First();

        string queryGameCenter = $"SELECT m.map_center AS MapCenter FROM game_configurations gc INNER JOIN maps m on gc.game_configuration_id = m.map_id and gc.game_configuration_id = {id}";
        string result = (await Context.Database
                .SqlQueryRaw<string>(queryGameCenter)
                .ToListAsync(cancellationToken))
                .First();
        gameConfiguration.MapCenter = Position.FromString(result);
        gameConfiguration.Teams = await GetTeamsByGameConfigurationIdAsync(id, cancellationToken);

        return gameConfiguration;
    }

    public async Task<List<PublicGameConfigurationViewModel>> GetAllPublicAsync(Guid accountId, CancellationToken cancellationToken)
    {
        string query = $"SELECT gc.game_configuration_id as Id, gc.game_name AS GameName, m.map_id AS MapId, gc.is_private AS IsPrivate FROM game_configurations gc INNER JOIN maps m ON gc.game_configuration_id = m.map_id AND (gc.\"AccountId\" != '{accountId}' AND gc.is_private = false)";
        return await Context.Database
            .SqlQueryRaw<PublicGameConfigurationViewModel>(query)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<PrivateGameConfigurationViewModel>> GetAllByAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        string query = $"SELECT gc.game_configuration_id as Id, gc.game_name AS GameName, m.map_id AS MapId, gc.is_private AS IsPrivate FROM game_configurations gc INNER JOIN maps m ON gc.game_configuration_id = m.map_id AND (gc.\"AccountId\" = '{accountId}')";
        return await Context.Database
            .SqlQueryRaw<PrivateGameConfigurationViewModel>(query)
            .ToListAsync(cancellationToken);
    }

    private async Task<List<TeamViewModel>> GetTeamsByGameConfigurationIdAsync(int gameConfigurationId, CancellationToken cancellationToken)
    {
        FormattableString teamQuery = $"SELECT t.team_id AS Id, t.color AS Color, t.name AS Name, t.nb_player AS NbPlayer from teams t where t.\"GameConfigurationId\" = {gameConfigurationId}";
        return await Context.Database.SqlQuery<TeamViewModel>(teamQuery).ToListAsync(cancellationToken);
    }
}