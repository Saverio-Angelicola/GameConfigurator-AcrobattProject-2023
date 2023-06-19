using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;

namespace Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;

public sealed class GameConfigurationDetailHandler : IQueryHandler<GameConfigurationDetail, GameConfigurationDetailViewModel>
{
    private readonly IGameConfigurationReadRepository _gameConfigurationReadRepository;
    
    public GameConfigurationDetailHandler(IGameConfigurationReadRepository gameConfigurationReadRepository)
    {
        _gameConfigurationReadRepository = gameConfigurationReadRepository;
    }

    public async Task<GameConfigurationDetailViewModel> HandleAsync(GameConfigurationDetail query, CancellationToken cancellationToken)
    {
        return await _gameConfigurationReadRepository.GetByIdAsync(query.Id, cancellationToken);
    }
}