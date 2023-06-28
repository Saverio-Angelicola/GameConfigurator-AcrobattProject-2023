using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Application.GameConfigurations.Features.GenerateGame;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.Application.Maps.Features.LoadMap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acrobatt.API.Controllers;

[Route("[controller]")]
public sealed class GameConfigurationController : AbstractController
{
    public GameConfigurationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : base(
        commandDispatcher, queryDispatcher)
    {
    }

    [HttpGet("{gameConfigurationId}")]
    [ProducesResponseType(typeof(GameConfigurationDetailViewModel), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Create(int gameConfigurationId, CancellationToken cancellationToken)
    {
        return Ok(await QueryDispatcher
            .DispatchAsync<GameConfigurationDetail, GameConfigurationDetailViewModel>(
            new GameConfigurationDetail(gameConfigurationId), cancellationToken));
    }

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(CreateGameConfigurationResult), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> Create(IFormFile mapFile, CreateGameConfigurationDto createGameConfigurationDto,
        CancellationToken cancellationToken)
    {
        MemoryStream map = await GetFileStream(mapFile, cancellationToken);
        CreateGameConfiguration command = new(map, createGameConfigurationDto);
        return Ok(await CommandDispatcher.DispatchAsync<CreateGameConfiguration, CreateGameConfigurationResult>(command, cancellationToken));
    }

    [HttpGet("public")]
    [ProducesResponseType(typeof(List<PublicGameConfigurationViewModel>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetAllPublicGameConfigurations(CancellationToken cancellationToken)
    {
         return Ok(await QueryDispatcher
             .DispatchAsync<GetPublicGameConfigurationsQuery, List<PublicGameConfigurationViewModel>>(new GetPublicGameConfigurationsQuery(), cancellationToken));
    }
    
    [HttpGet("private")]
    [ProducesResponseType(typeof(List<PrivateGameConfigurationViewModel>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetAllPrivateGameConfigurations(CancellationToken cancellationToken)
    {
         return Ok(await QueryDispatcher
             .DispatchAsync<GetPrivateGameConfigurationsQuery, List<PrivateGameConfigurationViewModel>>(new GetPrivateGameConfigurationsQuery(), cancellationToken));
    }

    [ProducesResponseType(typeof(JsonContent), StatusCodes.Status200OK)]
    [Authorize]
    [HttpGet("map/{gameConfigurationId}")]
    public async Task<IActionResult> LoadMap(int gameConfigurationId, CancellationToken cancellationToken)
    {
        LoadMapResult result = await CommandDispatcher.DispatchAsync<LoadMapCommand, LoadMapResult>(new LoadMapCommand(gameConfigurationId), cancellationToken);
        return File(result.Map, "application/json");
    }

    [Authorize]
    [HttpPost("{gameConfigurationId}/[action]")]
    public async Task<IActionResult> DownloadGame(int gameConfigurationId, CancellationToken cancellationToken)
    {
        GenerateGameResult result = await CommandDispatcher
            .DispatchAsync<GenerateGameCommand, GenerateGameResult>(new GenerateGameCommand(gameConfigurationId), cancellationToken);
        return File(result.GameFile, "application/zip", "gameserver.zip");
    }
}