using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;

public record CreateGameConfigurationResult(int GameConfigurationId) : ICommandResult
{
    
}