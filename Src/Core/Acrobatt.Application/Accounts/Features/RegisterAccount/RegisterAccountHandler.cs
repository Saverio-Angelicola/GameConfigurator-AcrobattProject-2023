using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;

namespace Acrobatt.Application.Accounts.Features.RegisterAccount;

public sealed class RegisterAccountHandler : ICommandHandler<RegisterAccount, RegisterAccountResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGuidProvider _guidProvider;
    private readonly IPasswordProvider _passwordProvider;
    
    public RegisterAccountHandler(IAccountRepository accountRepository, IGuidProvider guidProvider, IPasswordProvider passwordProvider)
    {
        _accountRepository = accountRepository;
        _guidProvider = guidProvider;
        _passwordProvider = passwordProvider;

    }

    public async Task<RegisterAccountResult> HandleAsync(RegisterAccount registerAccount, CancellationToken cancellationToken)
    {
        Account newAccount = Account.Create(_guidProvider.Generate(),registerAccount.FirstName, registerAccount.LastName, 
            registerAccount.Pseudo,registerAccount.Email, registerAccount.Password);
        
        newAccount.UpdatePassword(_passwordProvider.Hash(newAccount, newAccount.Password.Value));
        await _accountRepository.AddAsync(newAccount, cancellationToken);
        
        return new RegisterAccountResult(newAccount.Id);
    }
}