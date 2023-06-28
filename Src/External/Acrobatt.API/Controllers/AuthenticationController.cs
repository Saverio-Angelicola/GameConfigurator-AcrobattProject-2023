using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Microsoft.AspNetCore.Mvc;

namespace Acrobatt.API.Controllers;

[Route("[controller]")]
public sealed class AuthenticationController : AbstractController
{
    public AuthenticationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : base(
        commandDispatcher, queryDispatcher)
    {
    }

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(RegisterAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterAccount command, CancellationToken cancellationToken)
    {
        return Ok(await CommandDispatcher
            .DispatchAsync<RegisterAccount, RegisterAccountResult>(command, cancellationToken));
    }

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(LoginAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginAccount loginAccount, CancellationToken cancellationToken)
    {
        return Ok(await CommandDispatcher
            .DispatchAsync<LoginAccount, LoginAccountResult>(loginAccount, cancellationToken));
    }
}