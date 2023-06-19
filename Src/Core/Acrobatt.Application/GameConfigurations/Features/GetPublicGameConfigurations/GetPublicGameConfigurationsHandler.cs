using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;

public sealed class GetPublicGameConfigurationsHandler : IQueryHandler<GetPublicGameConfigurationsQuery, List<PublicGameConfigurationViewModel>>
{
    private readonly IAuthenticationGateway _authenticationGateway;
    private readonly IGameConfigurationReadRepository _gameConfigurationReadRepository;

    public GetPublicGameConfigurationsHandler(IAuthenticationGateway authenticationGateway, IGameConfigurationReadRepository gameConfigurationReadRepository)
    {
        _authenticationGateway = authenticationGateway;
        _gameConfigurationReadRepository = gameConfigurationReadRepository;
    }

    public async Task<List<PublicGameConfigurationViewModel>> HandleAsync(GetPublicGameConfigurationsQuery query, CancellationToken cancellationToken)
    {
        return await _gameConfigurationReadRepository.GetAllPublicAsync(_authenticationGateway.GetAuthenticateAccount().Id, cancellationToken);
    }
}