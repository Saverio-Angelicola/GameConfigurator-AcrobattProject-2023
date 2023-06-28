using Acrobatt.Application.Accounts.Exceptions;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Domain.Accounts;

namespace Acrobatt.Application.Accounts.Features.LoginAccount;

public sealed class LoginAccountHandler : ICommandHandler<LoginAccount, LoginAccountResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordProvider _passwordProvider;
    private readonly IJwtProvider _jwtProvider;

    public LoginAccountHandler(IAccountRepository accountRepository, IPasswordProvider passwordProvider, IJwtProvider jwtProvider)
    {
        _accountRepository = accountRepository;
        _passwordProvider = passwordProvider;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginAccountResult> HandleAsync(LoginAccount command, CancellationToken cancellationToken)
    {
        Account? account = await _accountRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException();
        }

        if (!_passwordProvider.Verify(account, command.Password, account.Password.Value))
        {
            throw new IncorrectPasswordException();
        }

        string token = _jwtProvider.CreateJwt(account.Id);
        
        return new LoginAccountResult(token, account.Id, account.Email, account.FirstName, account.LastName, account.Pseudo);
    }
}