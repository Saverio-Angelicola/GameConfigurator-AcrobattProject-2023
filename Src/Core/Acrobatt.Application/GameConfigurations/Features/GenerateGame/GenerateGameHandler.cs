using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;


namespace Acrobatt.Application.GameConfigurations.Features.GenerateGame;

public class GenerateGameHandler : ICommandHandler<GenerateGameCommand, GenerateGameResult>
{
    private readonly IGameGeneratorProvider _gameGeneratorProvider;
    private readonly IAccountRepository _accountRepository;

    public GenerateGameHandler(IGameGeneratorProvider gameGeneratorProvider, IAccountRepository accountRepository)
    {
        _gameGeneratorProvider = gameGeneratorProvider;
        _accountRepository = accountRepository;
    }

    public async Task<GenerateGameResult> HandleAsync(GenerateGameCommand command, CancellationToken cancellationToken)
    {
        GameConfiguration? gameConfiguration = await _accountRepository.GetGameConfigurationById(command.GameConfigurationId, cancellationToken);
        return new GenerateGameResult(await _gameGeneratorProvider.GenerateGameArchive(gameConfiguration, cancellationToken));
    }
}