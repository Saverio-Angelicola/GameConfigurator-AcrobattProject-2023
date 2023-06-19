using Acrobatt.Domain.Accounts.Enums;

namespace Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;

public record CreateGameConfigurationDto(string GameName,int MaxPlayers, int MaxFlags, double Duration, GameMode GameMode, bool IsPrivate, List<TeamDto> Teams, string MapName, bool MapVisibility, MapCenterDto MapCenter)
{
    
}