using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;

namespace Acrobatt.Application.Accounts.Features.UpdateAccount;

public sealed class UpdateAccountHandler : ICommandHandler<UpdateAccount, UpdateAccountResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthenticationGateway _authenticationGateway;
    
    public UpdateAccountHandler(IAccountRepository accountRepository, IAuthenticationGateway authenticationGateway)
    {
        _accountRepository = accountRepository;
        _authenticationGateway = authenticationGateway;
    }

    public async Task<UpdateAccountResult> HandleAsync(UpdateAccount updateAccount, CancellationToken cancellationToken)
    {
        Account account = _authenticationGateway.GetAuthenticateAccount();
        account.UpdateInformations(updateAccount.FirstName, updateAccount.LastName, updateAccount.Pseudo, updateAccount.Email);
        await _accountRepository.UpdateAsync(account, cancellationToken);
        return new UpdateAccountResult(updateAccount.FirstName, updateAccount.LastName, updateAccount.Pseudo, updateAccount.Email);
    }
}