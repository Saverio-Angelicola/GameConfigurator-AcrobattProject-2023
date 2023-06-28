using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.GameConfigurations.Features.GenerateGame;

public record GenerateGameCommand(int GameConfigurationId) : ICommand { }