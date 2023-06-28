using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.Maps.Features.LoadMap;

public sealed class LoadMapHandler : ICommandHandler<LoadMapCommand, LoadMapResult>
{
    private readonly IStorageProvider _storageProvider;
    private readonly IAccountRepository _accountRepository;

    public LoadMapHandler(IStorageProvider storageProvider, IAccountRepository accountRepository)
    {
        _storageProvider = storageProvider;
        _accountRepository = accountRepository;
    }

    public async Task<LoadMapResult> HandleAsync(LoadMapCommand command, CancellationToken cancellationToken)
    {
        Map? map = (await _accountRepository.GetGameConfigurationById(command.gameConfigurationId, cancellationToken))?.Map;
        return new LoadMapResult(await _storageProvider.GetFileAsync(map.MapName, cancellationToken));
    }
}