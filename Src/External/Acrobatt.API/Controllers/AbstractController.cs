using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Microsoft.AspNetCore.Mvc;

namespace Acrobatt.API.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected readonly ICommandDispatcher CommandDispatcher;
    protected readonly IQueryDispatcher QueryDispatcher;

    protected AbstractController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        CommandDispatcher = commandDispatcher;
        QueryDispatcher = queryDispatcher;
    }

    protected async Task<MemoryStream> GetFileStream(IFormFile file, CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new();
        await file.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream;
    }
}