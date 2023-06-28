using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts.Enums;

namespace Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;

public record CreateGameConfiguration(MemoryStream MapFile, CreateGameConfigurationDto CreateGameConfigurationDto) : ICommand
{
}