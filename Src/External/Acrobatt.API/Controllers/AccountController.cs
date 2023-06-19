using Acrobatt.Application.Accounts.Features.AccountDetails;
using Acrobatt.Application.Accounts.Features.DeleteAccount;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acrobatt.API.Controllers;

[Route("[controller]")]
public sealed class AccountController : AbstractController
{
    public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : base(commandDispatcher, queryDispatcher)
    {
    }

    [HttpPatch]
    [Authorize]
    [ProducesResponseType( typeof(UpdateAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateAccount command, CancellationToken cancellationToken)
    {
        return Ok(await CommandDispatcher
            .DispatchAsync<UpdateAccount, UpdateAccountResult>(command, cancellationToken));
    }
    
    [HttpDelete]
    [Authorize]
    [ProducesResponseType( typeof(DeleteAccountResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        return Ok(await CommandDispatcher
            .DispatchAsync<DeleteAccount, DeleteAccountResult>(new DeleteAccount(), cancellationToken));
    }
    
    [HttpGet("{accountId}")]
    [Authorize]
    [ProducesResponseType( typeof(AccountDetailsViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string accountId, CancellationToken cancellationToken)
    {
        return Ok(await QueryDispatcher
            .DispatchAsync<AccountDetails, AccountDetailsViewModel>(new AccountDetails(Guid.Parse(accountId)), cancellationToken));
    }
}