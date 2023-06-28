namespace Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;

public record PublicGameConfigurationViewModel(int Id, string GameName, int MapId, bool IsPrivate)
{
}