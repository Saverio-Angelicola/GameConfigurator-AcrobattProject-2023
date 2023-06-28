namespace Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;

public record PrivateGameConfigurationViewModel(int Id, string GameName, int MapId, bool IsPrivate)
{
}