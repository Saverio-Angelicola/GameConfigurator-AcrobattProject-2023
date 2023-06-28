using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;

namespace Acrobatt.Application.Commons.Contracts.ReadRepositories;

public interface IGameConfigurationReadRepository
{
    Task<GameConfigurationDetailViewModel> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<PublicGameConfigurationViewModel>> GetAllPublicAsync(Guid accountId, CancellationToken cancellationToken);
    Task<List<PrivateGameConfigurationViewModel>> GetAllByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    
}