using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Maps.Features.LoadMap;

public record LoadMapResult(Stream Map) : ICommandResult { }