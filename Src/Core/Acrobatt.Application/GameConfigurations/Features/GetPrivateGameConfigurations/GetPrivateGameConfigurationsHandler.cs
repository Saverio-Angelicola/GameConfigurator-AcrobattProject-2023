using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;

public sealed class GetPrivateGameConfigurationsHandler : IQueryHandler<GetPrivateGameConfigurationsQuery, List<PrivateGameConfigurationViewModel>>
{
    private readonly IAuthenticationGateway _authenticationGateway;
    private readonly IGameConfigurationReadRepository _gameConfigurationReadRepository;

    public GetPrivateGameConfigurationsHandler(IAuthenticationGateway authenticationGateway, IGameConfigurationReadRepository gameConfigurationReadRepository)
    {
        _authenticationGateway = authenticationGateway;
        _gameConfigurationReadRepository = gameConfigurationReadRepository;
    }

    public async Task<List<PrivateGameConfigurationViewModel>> HandleAsync(GetPrivateGameConfigurationsQuery query, CancellationToken cancellationToken)
    {
        return await _gameConfigurationReadRepository.GetAllByAccountAsync(_authenticationGateway.GetAuthenticateAccount().Id, cancellationToken);
    }
}