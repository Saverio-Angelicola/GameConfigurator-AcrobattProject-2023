using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.Accounts.Features.DeleteAccount;

public sealed class DeleteAccountHandler : ICommandHandler<DeleteAccount, DeleteAccountResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthenticationGateway _authenticationGateway;

    public DeleteAccountHandler(IAccountRepository accountRepository, IAuthenticationGateway authenticationGateway)
    {
        _accountRepository = accountRepository;
        _authenticationGateway = authenticationGateway;
    }

    public async Task<DeleteAccountResult> HandleAsync(DeleteAccount command, CancellationToken cancellationToken)
    {
        Account account = _authenticationGateway.GetAuthenticateAccount();
        await _accountRepository.DeleteAsync(account, cancellationToken);
        return new DeleteAccountResult(account.Id);
    }
}