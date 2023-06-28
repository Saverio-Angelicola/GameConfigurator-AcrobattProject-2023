using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;

public sealed class CreateGameConfigurationHandler : ICommandHandler<CreateGameConfiguration, CreateGameConfigurationResult>
{
    private readonly IAuthenticationGateway _authenticationGateway;
    private readonly IAccountRepository _accountRepository;
    private readonly IStorageProvider _storageProvider;

    public CreateGameConfigurationHandler(IAuthenticationGateway authenticationGateway,
        IAccountRepository accountRepository, IStorageProvider storageProvider)
    {
        _authenticationGateway = authenticationGateway;
        _accountRepository = accountRepository;
        _storageProvider = storageProvider;
    }

    public async Task<CreateGameConfigurationResult> HandleAsync(CreateGameConfiguration createGameConfiguration, CancellationToken cancellationToken)
    {
        Account account = _authenticationGateway.GetAuthenticateAccount();

        GameConfiguration gameConfiguration = CreateGameConfiguration(createGameConfiguration);

        account.AddGameConfiguration(gameConfiguration);

        await _storageProvider.UploadFileAsync(createGameConfiguration.CreateGameConfigurationDto.MapName,
            createGameConfiguration.MapFile, cancellationToken);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        return new CreateGameConfigurationResult(gameConfiguration.Id);
    }

    private GameConfiguration CreateGameConfiguration(CreateGameConfiguration createGameConfiguration)
    {
        Map map = new(createGameConfiguration.CreateGameConfigurationDto.MapName,
            createGameConfiguration.CreateGameConfigurationDto.MapVisibility, 
            new Position(createGameConfiguration.CreateGameConfigurationDto.MapCenter.X, createGameConfiguration.CreateGameConfigurationDto.MapCenter.Y));
        
        return GameConfiguration.Create(
            createGameConfiguration.CreateGameConfigurationDto.GameName,
            createGameConfiguration.CreateGameConfigurationDto.MaxPlayers,
            createGameConfiguration.CreateGameConfigurationDto.MaxFlags,
            createGameConfiguration.CreateGameConfigurationDto.Duration,
            createGameConfiguration.CreateGameConfigurationDto.GameMode,
            createGameConfiguration.CreateGameConfigurationDto.IsPrivate,
            map, AssignTeams(createGameConfiguration.CreateGameConfigurationDto.Teams));
    }

    private List<Team> AssignTeams(IEnumerable<TeamDto> teams)
    {
        return teams
            .Select(t => new Team(t.Color, t.NbPlayer, t.Name))
            .ToList();
    }
}