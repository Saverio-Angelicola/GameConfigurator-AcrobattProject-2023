using Acrobatt.Domain.Accounts.Entities;

namespace Acrobatt.Application.Commons.Contracts.Providers;

public interface IGameGeneratorProvider
{
    Task<Stream> GenerateGameArchive(GameConfiguration? gameConfiguration, CancellationToken cancellationToken);
}