using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts.Entities;

namespace Acrobatt.Infrastructure.InMemory.Providers;

public sealed class InMemoryGameGeneratorProvider : IGameGeneratorProvider
{
    public async Task<Stream> GenerateGameArchive(GameConfiguration? gameConfiguration, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return Stream.Null;
    }
}